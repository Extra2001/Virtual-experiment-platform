/************************************************************************************
    作者：荆煦添
    描述：提示器的UI抽象类
*************************************************************************************/
using HT.Framework;

public abstract class IndicatorBase : UILogicResident
{
    public abstract void ShowIndicator(string key, string message);
}
