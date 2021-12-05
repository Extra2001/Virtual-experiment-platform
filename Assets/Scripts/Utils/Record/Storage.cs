/************************************************************************************
    作者：荆煦添
    描述：提供游戏本地存储
*************************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using WisdomTree.Common.Function;

namespace Common
{
    /// <summary>
    /// 提供游戏本地存储
    /// </summary>
    public class Storage
    {
        readonly private string directory = null;
        /// <summary>
        /// 存储位置。0为PlayerPrefs，1为本地文件，2为上传到服务器
        /// </summary>
        readonly private int location;
        /// <summary>
        /// 创建一个Storage对象，对应指定ID的Storage
        /// </summary>
        /// <param name="id">Storage的ID</param>
        private Storage(int location)
        {
            this.location = location;
            directory = $"{Application.persistentDataPath}/LocalStorage/";
        }
        public static Storage LocalStorage
        {
            get => new Storage(1);
        }
        public static Storage RemoteStorage
        {
            get => new Storage(2);
        }
        public static Storage UnityStorage
        {
            get => new Storage(0);
        }
        public void GetAllKeys(Action<List<string>> responseAction)
        {
            if (location == 1)
            {
                responseAction?.Invoke(Directory.GetFiles(directory).ToList());
            }
            else if (location == 0)
            {
                responseAction(new List<string>());
            }
            else if (location == 2)
            {
                Communication.GetExperimentKeyUser(x =>
                {
                    responseAction?.Invoke(x);
                }, error: () => responseAction?.Invoke(new List<string>()));
            }
        }
        /// <summary>
        /// 获取Storage存储的对象，并使用自定义错误处理程序
        /// </summary>
        /// <typeparam name="T">映射的对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetStorage<T>(string key, Action<T> responseAction, Action<string> error, Func<T> initializer = null) where T : new()
        {
            if (location == 1)
            {
                string path = directory + key;
                var ret = SerializeHelper.DeSerialize(FileIOHelper.ReadJSONFile(path), initializer);
                responseAction?.Invoke(ret);
                return ret;
            }
            else if (location == 0)
            {
                var str = PlayerPrefs.GetString(key, "{}");
                var ret = SerializeHelper.DeSerialize(str, initializer);
                responseAction?.Invoke(ret);
                return ret;
            }
            else if (location == 2)
            {
                Communication.DownloadText(key, x =>
                {
                    var down = JsonConvert.DeserializeObject<Model>(x);
                    Debug.Log($"下载文本：{x}");
                    Debug.Log($"解析文本：{SerializeHelper.DeSerialize(down.data, initializer)}");
                    responseAction?.Invoke(SerializeHelper.DeSerialize(down.data, initializer));
                }, y => error?.Invoke(y));
            }
            return default;
        }
        /// <summary>
        /// 将对象存储到Storage中，并使用自定义错误处理程序
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">对象值，不可为自包含属性类型</param>
        public void SetStorage(string key, object values, Action<string> responseAction = null, Action<string> error = null, EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> errorHandler = null)
        {
            if (location == 1)
            {
                string path = directory + key;
                FileIOHelper.SaveFile(path, SerializeHelper.Serialize(values, errorHandler));
                responseAction?.Invoke("");
            }
            else if (location == 0)
            {
                PlayerPrefs.SetString(key, SerializeHelper.Serialize(values, errorHandler));
                responseAction?.Invoke("");
            }
            else if (location == 2)
            {
                Communication.UploadText(key, SerializeHelper.Serialize(values, errorHandler), x =>
                {
                    responseAction?.Invoke(x);
                }, y => error?.Invoke(y));
            }
        }
        /// <summary>
        /// 删除存储
        /// </summary>
        public void DeleteStorage(string key, Action<bool> responseAction = null)
        {
            if (location == 1)
            {
                string path = directory + key;
                FileIOHelper.DeleteFile(path);
                responseAction.Invoke(true);
            }
            else if (location == 0)
            {
                PlayerPrefs.DeleteKey(key);
                responseAction.Invoke(true);
            }
            else if (location == 2)
            {
                Communication.DeleteExperimentKeyUser(key, x => responseAction?.Invoke(x));
            }
        }
        /// <summary>
        /// 删除该存储方式下的所有内容
        /// </summary>
        public void DeleteSelf()
        {
            if (location == 1)
            {
                Directory.Delete(directory, true);
            }
            else if (location == 0)
            {
                PlayerPrefs.DeleteAll();
            }
            else if (location == 2)
            {
                Communication.GetExperimentKeyUser(x =>
                {
                    foreach (var item in x)
                    {
                        Communication.DeleteExperimentKeyUser(item, null);
                    }
                });
            }
        }
        /// <summary>
        /// 删除所有本地存储
        /// </summary>
        public static void DeleteAll()
        {
            try { LocalStorage.DeleteSelf(); } catch { }
            try { RemoteStorage.DeleteSelf(); } catch { }
            try { UnityStorage.DeleteSelf(); } catch { }
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

        public class Model
        {
            public int code { get; set; }
            public string data { get; set; }
            public string message { get; set; }
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
