using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class Storage
{
    public int id { get; }
    private string name { get; }
    private string directory = null;
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
    /// 游戏通用Storage
    /// </summary>
    public static Storage CommonStorage
    {
        get => new Storage("common");
    }
    public object this[string key, Type t]
    {
        get => JsonConvert.DeserializeObject(FileIOHelper.ReadJSONFile(directory + key), t);
        set => SetStorage(key, value);
    }
    public object this[string key]
    {
        set => SetStorage(key, value);
    }
    /// <summary>
    /// 获取Storage存储的对象，以类型T返回
    /// </summary>
    /// <typeparam name="T">映射的对象类型</typeparam>
    /// <param name="key">键</param>
    /// <returns></returns>
    public T GetStorage<T>(string key) where T : new()
    {
        string path = directory + key;
        var ret = JsonConvert.DeserializeObject<T>(FileIOHelper.ReadJSONFile(path), new JsonSerializerSettings()
        {
            Error = (sender, e) =>
            {
                e.ErrorContext.Handled = true;
                SetStorage(key, new T());
            }
        });
        if (ret == null)
            return new T();
        return ret;
    }
    public T GetStorage<T>(string key, Func<T> initializer) where T : new()
    {
        string path = directory + key;
        T whenError = new T();
        var ret = JsonConvert.DeserializeObject<T>(FileIOHelper.ReadJSONFile(path), new JsonSerializerSettings()
        {
            Error = (sender, e) =>
            {
                e.ErrorContext.Handled = true;
                whenError = initializer();
            }
        });
        if (ret == null)
            return whenError;
        return ret;
    }
    /// <summary>
    /// 将对象存储到Storage中
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="values">对象值，不可为自包含属性类型</param>
    public void SetStorage(string key, object values)
    {
        string path = directory + key;
        FileIOHelper.SaveFile(path, JsonConvert.SerializeObject(values, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Error = (sender, e) =>
            {
                Debug.Log($"JsonSerializationError: {e.ErrorContext.Error}");
                e.ErrorContext.Handled = true;
            }
        }));
    }
    public void SetStorage(string key, object values, EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> errorHandler)
    {
        string path = directory + key;
        FileIOHelper.SaveFile(path, JsonConvert.SerializeObject(values, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Error = errorHandler
        }));
    }
    public void DeleteStorage(string key)
    {
        string path = directory + key;
        FileIOHelper.DeleteFile(path);
    }

    public void DeleteSelf()
    {
        Directory.Delete(directory, true);
    }

    public static void DeleteAll()
    {
        FileIOHelper.DeleteDirectory($"{Application.persistentDataPath}/LocalStorage");
    }

    protected static class FileIOHelper
    {
        internal static string ReadJSONFile(string Path)
        {
            string json = ReadFile(Path);
            if (json == null) return "{}";
            else return json;
        }
        internal static string ReadFile(string Path)
        {
            if (!File.Exists(Path))
                return null;
            using (StreamReader sr = new StreamReader(Path))
            {
                if (sr == null)
                    return null;
                return sr.ReadToEnd();
            }
        }
        internal static void SaveFile(string path, string data)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path).Close();
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(data);
            }
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
