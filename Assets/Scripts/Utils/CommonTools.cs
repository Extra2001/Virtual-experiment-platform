using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using HT.Framework;

public static class CommonTools
{
    public static List<Type> GetSubClassNames(Type parentType)
    {
        var subTypeList = new List<Type>();
        var assembly = parentType.Assembly;
        var assemblyAllTypes = assembly.GetTypes();
        foreach (var itemType in assemblyAllTypes)
        {
            List<string> hh = new List<string>();
            Type tmp = itemType;
            for (; tmp.BaseType != null; tmp = tmp.BaseType)
                hh.Add(tmp.BaseType.Name);
            if (hh.Contains(parentType.Name))
            {
                subTypeList.Add(itemType);
            }
        }
        return subTypeList;
    }

    public static object CreateInstance(this Type instrument)
    {
        return Activator.CreateInstance(instrument);
    }

    public static InstrumentBase CreateInstrumentInstance(this Type instrument)
    {
        return Activator.CreateInstance(instrument) as InstrumentBase;
    }

    private static Dictionary<string, Sprite> spritePool = new Dictionary<string, Sprite>();

    public static Sprite GetSprite(string path)
    {
        if (spritePool.ContainsKey(path))
            return spritePool[path];
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();
            Sprite ret = GetSprite(bytes);
            spritePool.Add(path, ret);
            return ret;
        }
    }

    public static Sprite GetSprite(byte[] bytes)
    {
        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(bytes);

        Sprite ret = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return ret;
    }

    // 不改变高度
    public static void FitHeight(this Image image, Sprite sprite)
    {
        image.sprite = sprite;
        var hh = image.gameObject.rectTransform().sizeDelta;
        hh.x = (float)sprite.texture.width / sprite.texture.height * hh.y;
        image.gameObject.rectTransform().sizeDelta = hh;
    }

    // 不改变宽度
    public static void FitWidth(this Image image, Sprite sprite)
    {
        image.sprite = sprite;
        var hh = image.gameObject.rectTransform().sizeDelta;
        hh.y = (float)sprite.texture.height / sprite.texture.width * hh.x;
        image.gameObject.rectTransform().sizeDelta = hh;
    }

    public static double GetExpressionExecuted(this List<FormulaNode> nodes, string rootGUID = "base")
    {
        return Javascript.Eval(GetExpression(nodes, rootGUID));
    }

    public static string GetExpression(this List<FormulaNode> nodes, string rootGUID = "base")
    {
        var curCell = nodes.Where(x => x.GUID.Equals(rootGUID)).Last();
        var value = curCell.value;
        foreach (var item in curCell.ReplaceFlags)
        {
            var subCell = nodes.Where(x => x.GUID.Equals(item)).Last();
            value = value.Replace(item, $"({GetExpression(nodes, item)})");
        }
        return value;
    }

    public static bool AlmostEqual(this double value1, double value2)
    {
        return Math.Abs(value1 - value2) < (value1 * 0.005);//误差允许0.5%
    }

    public static MyVector3 GetMyVector(this Vector3 vector3)
    {
        return new MyVector3()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };
    }

    public static MyVector4 GetMyVector(this Quaternion vector3)
    {
        return new MyVector4()
        {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z,
            w = vector3.w
        };
    }

    public static List<double> ToDouble(this List<string> list)
    {
        return list.Select(x => Convert.ToDouble(x)).ToList();
    }
}
