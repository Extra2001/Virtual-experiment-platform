/************************************************************************************
    作者：荆煦添
    描述：公式编辑器方块绑定数据
*************************************************************************************/
using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class FormulaCell : HTBehaviour
{
    /// <summary>
    /// 方块的唯一ID
    /// </summary>
    public string thisGUID = "";
    /// <summary>
    /// 方块的替换部分GUIDs
    /// </summary>
    public Dictionary<Button, string> ReplaceFlags = new Dictionary<Button, string>();
    /// <summary>
    /// 方块的第一个可点击部分
    /// </summary>
    public Button Value1;
    /// <summary>
    /// 方块的第二个可点击部分
    /// </summary>
    public Button Value2;
    /// <summary>
    /// 方块内部计算值
    /// </summary>
    public string value;
    /// <summary>
    /// 方块名称显示器
    /// </summary>
    public Text NameShower;
    /// <summary>
    /// 生成方块替换GUID
    /// </summary>
    /// <returns></returns>
    public string GenerateReplaceFlags()
    {
        ReplaceFlags.Clear();
        if (Value1 != null)
            ReplaceFlags.Add(Value1, "{0}");
        if (Value2 != null)
            ReplaceFlags.Add(Value2, "{1}");
        for (int i = 0; i < ReplaceFlags.Count; i++)
        {
            var item = ReplaceFlags.ElementAt(i);
            var str = Guid.NewGuid().ToString("N");
            value = value.Replace(item.Value, str);
            ReplaceFlags[item.Key] = str;
        }
        return value;
    }
}