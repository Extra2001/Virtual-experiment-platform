using System;

namespace Utils.Record
{
    /// <summary>
    /// 默认存档数据模型类
    /// </summary>
    [Serializable]
    public class Record
    {
        public Record() { }

        public Record(int id)
        {
            info.id = id;
            info.time = DateTime.Now;
            info.title = $"存档{id}";
        }

        public Record(int id, string name)
        {
            info.id = id;
            info.time = DateTime.Now;
            info.title = name;
        }

        public RecordInfo info { get; set; } = new RecordInfo();
    }
}
