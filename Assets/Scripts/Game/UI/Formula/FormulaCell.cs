using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class FormulaCell : HTBehaviour
{
    public VaribleExpression varibleExpression = 0;

    public string thisGUID = "";

    public Dictionary<Button, string> ReplaceFlags = new Dictionary<Button, string>();

    public Button Value1;

    public Button Value2;

    public string value;

    public string GenerateReplaceFlags()
    {
        ReplaceFlags.Clear();
        if (Value1 != null)
            ReplaceFlags.Add(Value1, "{0}");
        if (Value2 != null)
            ReplaceFlags.Add(Value2, "{1}");
        foreach (var item in ReplaceFlags)
        {
            var str = Guid.NewGuid().ToString("N");
            value = value.Replace(item.Value, str);
            ReplaceFlags[item.Key] = str;
        }
        return value;
    }
}