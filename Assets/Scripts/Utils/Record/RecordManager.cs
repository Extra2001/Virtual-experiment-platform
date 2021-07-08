using System.Collections.Generic;
using System;
using HT.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class RecordManager
{
    /// <summary>
    /// 存档信息列表
    /// </summary>
    public static List<RecordInfo> recordInfos
    {
        get => GetRecordInfos();
    }
    /// <summary>
    /// 存档索引器
    /// </summary>
    /// <param name="Id">存档ID</param>
    public static RecordIndexor records
    {
        get => new RecordIndexor();
    }
    /// <summary>
    /// 工作存档
    /// </summary>
    public static Record tempRecord
    {
        get
        {
            if (_tempRecord == null)
                _tempRecord = GetRecord(-2);
            _tempRecord.info.id = -2;
            return _tempRecord;
        }
    }

    private static Storage recordStorage = new Storage(0);
    private static Record _tempRecord = null;

    // 以下为存档的扩展方法
    public static void Save(this Record record)
    {
        if (record.info.id.Equals(-2))
        {
            recordStorage.SetStorage($"Record{record.info.id}", record);
            return;
        }
        SaveRecord(record);
    }
    public static void Delete(this Record record)
    {
        record = new Record()
        {
            info = record.info
        };
        DeleteRecord(record.info.id);
    }
    public static void Load(this Record record)
    {
        LoadToTempRecord(record);
        PauseManager.Instance.Continue();
        GameManager.Instance.ContinueExp();
    }
    /// <summary>
    /// 加载到工作存档
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    private static Record LoadToTempRecord(this Record record)
    {
        Initializer.InitializeObjects();
        var ret = record.DeepCopy<Record>();
        ret.info = tempRecord.info;
        _tempRecord = ret;
        Main.m_Event.Throw<ChangeRecordEventHandler>();
        return ret;
    }

    /// <summary>
    /// 清空工作存档
    /// </summary>
    /// <returns></returns>
    public static void ClearTempRecord()
    {
        _tempRecord = new Record(-2);
    }
    /// <summary>
    /// 覆盖保存存档内容
    /// </summary>
    public static void SaveRecord(Record record)
    {
        var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
        int index = list.FindIndex(x => record.info.id.Equals(x.id));
        if (index == -1)
            list.Add(record.info);
        else
            list[index] = record.info;

        recordStorage.SetStorage($"Record{record.info.id}", record);
        Storage.CommonStorage.SetStorage("RecordInfo", list);
        Main.m_Event.Throw<RecordUpdateEventHandler>();
    }
    /// <summary>
    /// 深拷贝
    /// </summary>
    public static T DeepCopy<T>(this object obj)
    {
        object retval;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            retval = bf.Deserialize(ms);
            ms.Close();
        }
        return (T)retval;
    }

    /// <summary>
    /// 获取第一个没有存档的ID
    /// </summary>
    public static int GetFirstNone()
    {
        var _records = recordInfos;
        for (int i = 1; ; i++)
        {
            if (_records.FindIndex(x => i.Equals(x.id)) == -1)
                return i;
        }
    }
    /// <summary>
    /// 是否存在指定ID的存档
    /// </summary>
    /// <param name="Id">存档ID</param>
    public static bool RecordContains(int Id)
    {
        var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
        int index = list.FindIndex(x => Id.Equals(x.id));
        if (index == -1)
            return false;
        return true;
    }

    /// <summary>
    /// 获取存档信息列表
    /// </summary>
    private static List<RecordInfo> GetRecordInfos()
    {
        return Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
    }
    /// <summary>
    /// 获取存档
    /// </summary>
    /// <param name="Id">存档ID</param>
    private static Record GetRecord(int Id)
    {
        return recordStorage.GetStorage<Record>($"Record{Id}");
    }
    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="Id">存档ID</param>
    private static void DeleteRecord(int Id)
    {
        var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
        int index = list.FindIndex(x => Id.Equals(x.id));
        if (index != -1)
            list.RemoveAt(index);
        Storage.CommonStorage.SetStorage("RecordInfo", list);
        recordStorage.DeleteStorage($"Record{Id}");

        Main.m_Event.Throw<RecordUpdateEventHandler>();
    }
    public class RecordIndexor
    {
        public Record this[int Id]
        {
            get => GetRecord(Id);
            set => SaveRecord(value);
        }
    }
}

/// <summary>
/// 存档信息模型类
/// 用来管理所有存档
/// </summary>
[Serializable]
public class RecordInfo
{
    public int id { get; set; }
    public string title { get; set; }
    public DateTime time { get; set; } = DateTime.Now;
    public string timeString { get => time.ToLocalTime().ToString("F"); }
}
