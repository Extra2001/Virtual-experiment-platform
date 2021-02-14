﻿namespace HT.Framework
{
    /// <summary>
    /// 任务点开始事件
    /// </summary>
    public sealed class EventTaskPointStart : EventHandlerBase
    {
        /// <summary>
        /// 任务点对象
        /// </summary>
        public TaskPointBase TaskPoint;

        /// <summary>
        /// 填充数据，所有属性、字段的初始化工作可以在这里完成
        /// </summary>
        public EventTaskPointStart Fill(TaskPointBase taskPoint)
        {
            TaskPoint = taskPoint;
            return this;
        }

        /// <summary>
        /// 重置引用，当被引用池回收时调用
        /// </summary>
        public override void Reset()
        {
            TaskPoint = null;
        }
    }
}