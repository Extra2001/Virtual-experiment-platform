using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class FormulaCell : HTBehaviour
{
    public string thisGUID = "";

    public Dictionary<Button, string> ReplaceFlags = new Dictionary<Button, string>();

    public Button Value1;

    public Button Value2;

    public string value;

    public Text NameShower;

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