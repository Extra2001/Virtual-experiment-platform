/************************************************************************************
    作者：荆煦添
    描述：仪器信息基础模型，用于右键显示
*************************************************************************************/
using System;

[Serializable]
public class InstrumentInfoModel
{
    public bool Valid = false;

    public Type instrumentType = null;

    public MyVector3 position = new MyVector3();
    public MyVector4 rotation = new MyVector4();

    public double MainValue = double.MinValue;
    public double RandomErrorLimit = double.MinValue;
}