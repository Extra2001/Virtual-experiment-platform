/************************************************************************************
    作者：张峻凡
    描述：用户选择桌子开始实验事件
*************************************************************************************/
using HT.Framework;
/// <summary>
/// 用户选择桌子开始实验事件
/// </summary>
public class SitdownEventHandler : EventHandlerBase
{
    /// <summary>
    /// 获得人所在位置，确保仪器和被测物体生成在人面前
    /// </summary>
    public SitdownEventHandler Fill(float x, float y, float z)
    {
        RecordManager.tempRecord.instrumentStartPosition[0] = x;
        RecordManager.tempRecord.instrumentStartPosition[1] = y;
        RecordManager.tempRecord.instrumentStartPosition[2] = z;

        RecordManager.tempRecord.objectStartPosition[0] = x;
        RecordManager.tempRecord.objectStartPosition[1] = y + .2f;
        RecordManager.tempRecord.objectStartPosition[2] = z + 6f;

        return this;
    }

    /// <summary>
    /// 重置引用，当被引用池回收时调用
    /// </summary>
    public override void Reset()
    {

    }
}
