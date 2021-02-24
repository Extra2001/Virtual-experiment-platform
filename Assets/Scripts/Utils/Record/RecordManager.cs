using System.Collections.Generic;
using System;
using HT.Framework;

namespace Utils.Record
{
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
        /// 获取默认存档
        /// </summary>
        /// <param name="Id">存档ID</param>
        public static RecordIndexor defaultRecord
        {
            get => new RecordIndexor();
        }
        /// <summary>
        /// 当前的存档Id
        /// </summary>
        public static int currentRecordId
        {
            get
            {
                if (_currentRecordId == -1)
                    _currentRecordId = Storage.CommonStorage.GetStorage<HH>("currentRecordId").currentRecordId;
                return _currentRecordId;
            }
            set
            {
                _currentRecordId = value;
                Storage.CommonStorage.SetStorage("currentRecordId", new HH() { currentRecordId = value });
            }
        }
        public static DefaultRecord currentDefaultRecord
        {
            get => GetRecord(currentRecordId);
        }
        private static int _currentRecordId = -1;
        /// <summary>
        /// 保存由存档管理器收集默认存档内容
        /// </summary>
        /// <param name="recordInfo">存档信息</param>
        public static void SaveRecord(RecordInfo recordInfo)
        {
            SaveRecord(recordInfo, CollectRecordData(recordInfo));
        }
        /// <summary>
        /// 保存已收集好的默认存档内容
        /// </summary>
        public static void SaveRecord(RecordInfo recordInfo, DefaultRecord record)
        {
            if (recordInfo == null)
                recordInfo = DefaultRecord.GenRecordInfo(GetFirstNone());
            if (string.IsNullOrEmpty(recordInfo.title))
                recordInfo = DefaultRecord.GenRecordInfo(recordInfo.recordId);
            var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
            int index = list.FindIndex(x => recordInfo.recordId.Equals(x.recordId));
            Storage recordStorage = new Storage(recordInfo.recordId);
            if (index == -1)
                list.Add(recordInfo);
            else
                list[index] = recordInfo;
            recordStorage.SetStorage("defaultRecord", record);
            Storage.CommonStorage.SetStorage("RecordInfo", list);
            Main.m_Event.Throw<RecordUpdateEventHandler>();
        }
        /// <summary>
        /// 获取存档信息列表
        public static List<RecordInfo> GetRecordInfos()
        {
            return Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
        }
        /// <summary>
        /// 获取存档信息列表
        public static RecordInfo GetRecordInfo(int Id)
        {
            return Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo").Find(x => Id.Equals(x.recordId));
        }
        /// <summary>
        /// 获取默认存档
        /// </summary>
        /// <param name="Id">存档ID</param>
        public static DefaultRecord GetRecord(int Id)
        {
            if (!RecordContains(Id))
                return null;
            Storage recordStorage = new Storage(Id);
            return recordStorage.GetStorage<DefaultRecord>("defaultRecord");
        }
        /// <summary>
        /// 将存档从存档信息中删除 不会实际删除文件
        /// </summary>
        /// <param name="Id">存档ID</param>
        public static void DeleteRecord(int Id)
        {
            var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
            int index = list.FindIndex(x => Id.Equals(x.recordId));
            if (index != -1)
                list.RemoveAt(index);
            Storage.CommonStorage.SetStorage("RecordInfo", list);
            Storage storage = new Storage(Id);
            storage.DeleteStorage();
            Main.m_Event.Throw<RecordUpdateEventHandler>();
        }
        /// <summary>
        /// 获取第一个没有存档的ID
        /// </summary>
        public static int GetFirstNone()
        {
            var _records = recordInfos;
            for (int i = 1; ; i++)
            {
                if (_records.FindIndex(x => i.Equals(x.recordId)) == -1)
                    return i;
            }
        }
        /// <summary>
        /// 获取存档其他附件
        /// </summary>
        /// <typeparam name="T">数据模型类</typeparam>
        /// <param name="Id">存档ID</param>
        /// <param name="name">附件名称</param>
        public static T GetAttachments<T>(int Id, string name) where T : new()
        {
            if (!RecordContains(Id))
                return default(T);
            Storage recordStorage = new Storage(Id);
            return recordStorage.GetStorage<T>(name);
        }
        /// <summary>
        /// 保存其他附件
        /// </summary>
        /// <typeparam name="T">数据模型类</typeparam>
        /// <param name="Id">存档ID</param>
        /// <param name="name">附件名称</param>
        /// <param name="values">内容</param>
        public static void SetAttachments<T>(int Id, string name, T values) where T : new()
        {
            Storage recordStorage = new Storage(Id);
            recordStorage.SetStorage(name, values);
        }
        /// <summary>
        /// 管理器内置默认存档数据收集器
        /// </summary>
        /// <param name="recordInfo">存档信息</param>
        private static DefaultRecord CollectRecordData(RecordInfo recordInfo)
        {
            var record = new DefaultRecord();
            // 在此处收集存档信息

            return record;
        }
        /// <summary>
        /// 是否存在指定ID的存档
        /// </summary>
        /// <param name="Id">存档ID</param>
        public static bool RecordContains(int Id)
        {
            var list = Storage.CommonStorage.GetStorage<List<RecordInfo>>("RecordInfo");
            int index = list.FindIndex(x => Id.Equals(x.recordId));
            if (index == -1)
                return false;
            return true;
        }
        public class RecordIndexor
        {
            public DefaultRecord this[int Id]
            {
                get => GetRecord(Id);
                set => SaveRecord(GetRecordInfo(Id), value);
            }
        }
        private class HH
        {
            public int currentRecordId { get; set; }
        }
    }

    /// <summary>
    /// 存档信息模型类
    /// 用来管理所有存档
    /// </summary>
    public class RecordInfo
    {
        public int recordId { get; set; }
        public string title { get; set; }
        public DateTime time { get; set; }
        public string timeString { get => time.ToLocalTime().ToString("F"); }
    }
}