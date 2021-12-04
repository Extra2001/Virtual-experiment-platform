/************************************************************************************
    作者：荆煦添
    描述：存档管理器
*************************************************************************************/
using System.Collections.Generic;
using System;
using HT.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Common;
using System.Linq;

public static class RecordManager
{
    private static List<RecordInfo> _recordInfos = null;
    /// <summary>
    /// 存档信息列表
    /// </summary>
    public static List<RecordInfo> recordInfos
    {
        get
        {
            if (_recordInfos == null)
                UpdateRecordInfos(null);
            return _recordInfos ?? new List<RecordInfo>();
        }
    }
    /// <summary>
    /// 工作存档
    /// </summary>
    public static Record tempRecord
    {
        get
        {
            if (_tempRecord == null)
                _tempRecord = GetRecord(new RecordInfo() { id = -2 }, null);
            _tempRecord.info.id = -2;
            return _tempRecord;
        }
    }

    private static Record _tempRecord = null;

    // 以下为存档的扩展方法
    /// <summary>
    /// 写入该存档到硬盘
    /// </summary>
    public static void Save(this Record record, Action done = null, Action error = null)
    {
        SaveRecord(record, done, error);
    }
    /// <summary>
    /// 删除该存档
    /// </summary>
    public static void Delete(this Record record, Action done = null, Action error = null)
    {
        record = new Record()
        {
            info = record.info
        };
        DeleteRecord(record.info, done, error);
    }
    /// <summary>
    /// 加载该存档到工作存档并取消暂停状态
    /// </summary>
    public static void Load(this Record record)
    {
        LoadToTempRecord(record);
        PauseManager.Instance.Continue(true);
        GameManager.Instance.ContinueExp();
    }
    /// <summary>
    /// 导入外来存档
    /// </summary>
    /// <param name="rec"></param>
    public static RecordInfo Import(string rec)
    {
        var record = JsonConvert.DeserializeObject<Record>(rec);
        var info = record.info;
        if (info == null) throw new Exception();
        info.id = GetFirstNone();
        info.title = "【导入】" + info.title;
        SaveRecord(record);
        return info;
    }
    /// <summary>
    /// 加载该存档到工作存档
    /// </summary>
    private static Record LoadToTempRecord(Record record)
    {
        Main.m_Event.Throw<BeforeClearTempRecordEventHandler>();
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
    public static void ClearTempRecord()
    {
        Main.m_Event.Throw<BeforeClearTempRecordEventHandler>();
        _tempRecord = new Record(-2);
        Main.m_Event.Throw<ChangeRecordEventHandler>();
    }
    /// <summary>
    /// 覆盖保存存档内容
    /// </summary>
    public static void SaveRecord(Record record, Action done = null, Action error = null)
    {
        if (record.info.id.Equals(-2))
        {
            Storage.UnityStorage.SetStorage($"tempRecord", record);
            done?.Invoke();
            return;
        }
        Storage.RemoteStorage.SetStorage(RecordInfoToName(record.info), record, responseAction: x =>
        {
            UpdateRecordInfos(y =>
            {
                Main.m_Event.Throw<RecordUpdateEventHandler>();
                done?.Invoke();
            });
        }, error: y => error?.Invoke());
    }
    /// <summary>
    /// 获取存档
    /// </summary>
    /// <param name="Id">存档ID</param>
    public static Record GetRecord(int id, Action<Record> done, Action<string> error = null)
    {
        return GetRecord(_recordInfos.Find(x => x.id == id), done, error);
    }
    public static Record GetRecord(RecordInfo info, Action<Record> done, Action<string> error = null)
    {
        if(info.id == -2)
            return Storage.UnityStorage.GetStorage<Record>("tempRecord", null, null);
        Storage.RemoteStorage.GetStorage(RecordInfoToName(info), done, error);
        return null;
    }
    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="Id">存档ID</param>
    public static void DeleteRecord(int id, Action done = null, Action error = null)
    {
        DeleteRecord(_recordInfos.Find(x => x.id == id), done, error);
    }
    public static void DeleteRecord(RecordInfo info, Action done = null, Action error = null)
    {
        Storage.RemoteStorage.DeleteStorage(RecordInfoToName(info), x =>
        {
            if (x)
            {
                UpdateRecordInfos(y =>
                {
                    Main.m_Event.Throw<RecordUpdateEventHandler>();
                    done?.Invoke();
                });
            }
            else error?.Invoke();
        });
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
        var list = _recordInfos;
        int index = list.FindIndex(x => Id.Equals(x.id));
        if (index == -1)
            return false;
        return true;
    }
    /// <summary>
    /// 获取存档信息列表
    /// </summary>
    public static void UpdateRecordInfos(Action<List<RecordInfo>> action)
    {
        Storage.RemoteStorage.GetAllKeys(x =>
        {
            _recordInfos = x.Select(y => NameToRecordInfo(y)).ToList();
            action?.Invoke(_recordInfos);
        });
    }
    private static RecordInfo NameToRecordInfo(string name)
    {
        var ret = new RecordInfo();
        var split = name.Split('|');
        ret.id = Convert.ToInt32(split[0]);
        ret.time = DateTime.Parse(split[1]);
        ret.title = split[2];
        return ret;
    }
    private static string RecordInfoToName(RecordInfo recordInfo)
    {
        return string.Join("|", recordInfo.id, recordInfo.time.ToString(), recordInfo.title);
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
