/************************************************************************************
    作者：荆煦添
    描述：提供游戏本地存储
*************************************************************************************/
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 提供游戏本地存储
    /// </summary>
    public class Storage
    {
        public int id { get; }
        private string name { get; }
        private string directory = null;
        /// <summary>
        /// 存储位置。0为PlayerPrefs，1为本地文件，2为上传到服务器
        /// </summary>
        private int location;
        /// <summary>
        /// 创建一个Storage对象，对应指定ID的Storage
        /// </summary>
        /// <param name="id">Storage的ID</param>
        public Storage(int id)
        {
            this.id = id;
            directory = $"{Application.persistentDataPath}/LocalStorage/{id}/";
        }
        private Storage(string name)
        {
            id = -1;
            this.name = name;
            directory = $"{Application.persistentDataPath}/LocalStorage/{this.name}/";
        }
        /// <summary>
        /// 公共Storage
        /// </summary>
        public static Storage CommonStorage
        {
            get => new Storage("common");
        }
        /// <summary>
        /// 用指定的key访问存储的索引器
        /// </summary>
        public object this[string key, Type t]
        {
            get => JsonConvert.DeserializeObject(FileIOHelper.ReadJSONFile(directory + key), t);
            set => SetStorage(key, value);
        }
        /// <summary>
        /// 用指定的key访问存储的索引器
        /// </summary>
        public object this[string key]
        {
            set => SetStorage(key, value);
        }
        /// <summary>
        /// 获取Storage存储的对象，并使用自定义错误处理程序
        /// </summary>
        /// <typeparam name="T">映射的对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetStorage<T>(string key, Func<T> initializer = null) where T : new()
        {
            string path = directory + key;
            return SerializeHelper.DeSerialize(FileIOHelper.ReadJSONFile(path), initializer);
        }
        /// <summary>
        /// 将对象存储到Storage中，并使用自定义错误处理程序
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">对象值，不可为自包含属性类型</param>
        public void SetStorage(string key, object values, EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> errorHandler = null)
        {
            string path = directory + key;
            FileIOHelper.SaveFile(path, SerializeHelper.Serialize(values, errorHandler));
        }
        /// <summary>
        /// 删除存储
        /// </summary>
        public void DeleteStorage(string key)
        {
            string path = directory + key;
            FileIOHelper.DeleteFile(path);
        }
        /// <summary>
        /// 删除自己
        /// </summary>
        public void DeleteSelf()
        {
            Directory.Delete(directory, true);
        }
        /// <summary>
        /// 删除所有本地存储
        /// </summary>
        public static void DeleteAll()
        {
            FileIOHelper.DeleteDirectory($"{Application.persistentDataPath}/LocalStorage");
        }
        /// <summary>
        /// 序列化+加密器
        /// </summary>
        protected static class SerializeHelper
        {
            public static string Serialize(object value, EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> errorHandler)
            {
                string data = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    Error = errorHandler ?? ((sender, e) =>
                    {
                        Debug.Log($"JsonSerializationError: {e.ErrorContext.Error}");
                        e.ErrorContext.Handled = true;
                    })
                });
                return new SymmetricMethod().Encrypto(data);
            }

            public static T DeSerialize<T>(string value, Func<T> initializer) where T : new()
            {
                string data;
                try { data = new SymmetricMethod().Decrypto(value); }
                catch { data = "{}"; }
                T whenError = new T();
                try
                {
                    var ret = JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                    if (ret == null)
                        return whenError;
                    return ret;
                }
                catch
                {
                    whenError = initializer == null ? new T() : initializer.Invoke();
                    return whenError;
                }
            }
        }
        /// <summary>
        /// 文件IO
        /// </summary>
        protected static class FileIOHelper
        {
            internal static string ReadJSONFile(string path)
            {
                string json = ReadFile(path);
                if (json == null) return "{}";
                else return json;
            }
            internal static string ReadFile(string path)
            {
                if (!File.Exists(path))
                    return null;
                return File.ReadAllText(path);
            }
            internal static void SaveFile(string path, string data)
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, data);
            }
            internal static void DeleteFile(string path)
            {
                File.Delete(path);
            }

            internal static void DeleteDirectory(string path)
            {
                Directory.Delete(path, true);
            }
        }
    }
}
