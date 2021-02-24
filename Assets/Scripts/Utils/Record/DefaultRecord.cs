using System;

namespace Utils.Record
{
    /// <summary>
    /// 默认存档数据模型类
    /// </summary>
    public class DefaultRecord
    {
        public static RecordInfo GenRecordInfo(int id, string title)
        {
            return new RecordInfo()
            {
                recordId = id,
                title = title,
                time = DateTime.Now
            };
        }

        public static RecordInfo GenRecordInfo(int id)
        {
            return GenRecordInfo(id, $"存档 {id}");
        }
    }
}
