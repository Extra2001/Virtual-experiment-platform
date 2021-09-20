/************************************************************************************
    作者：荆煦添
    描述：公共工具函数
*************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using HT.Framework;
using System.Collections;

public static class CommonTools
{
    /// <summary>
    /// 获取一个类的所有子类，主要用于获取已定义的所有仪器
    /// </summary>
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
    /// <summary>
    /// 根据Type创建仪器模型的实例
    /// </summary>
    public static InstrumentBase CreateInstrumentInstance(this Type instrument)
    {
        return Activator.CreateInstance(instrument) as InstrumentBase;
    }
    /// <summary>
    /// 图片缓存，防止产生大量IO造成程序卡死
    /// </summary>
    private static Dictionary<string, Sprite> spritePool = new Dictionary<string, Sprite>();
    private static Dictionary<string, byte[]> bytesPool = new Dictionary<string, byte[]>();
    public static byte[] GetBytes(string path)
    {
        if (bytesPool.ContainsKey(path)) return bytesPool[path];
        byte[] bytes;
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(0, SeekOrigin.Begin);
            bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Close();
        }
        bytesPool.Add(path, bytes);
        return bytes;
    }
    /// <summary>
    /// 根据路径加载图片到Sprite
    /// </summary>
    public static Sprite GetSprite(string path)
    {
        if (spritePool.ContainsKey(path))
            return spritePool[path];
        if (bytesPool.ContainsKey(path))
        {
            spritePool.Add(path, GetSprite(bytesPool[path]));
            return spritePool[path];
        }
        Sprite ret = GetSprite(GetBytes(path));
        spritePool.Add(path, ret);
        return ret;
    }
    /// <summary>
    /// 根据二进制信息加载图片到Sprite
    /// </summary>
    public static Sprite GetSprite(byte[] bytes)
    {
        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(bytes);

        Sprite ret = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return ret;
    }
    /// <summary>
    /// 不改变高度适应图片大小
    /// </summary>
    /// <param name="image"></param>
    /// <param name="sprite"></param>
    public static void FitHeight(this Image image, Sprite sprite)
    {
        image.sprite = sprite;
        var hh = image.gameObject.rectTransform().sizeDelta;
        hh.x = (float)sprite.texture.width / sprite.texture.height * hh.y;
        image.gameObject.rectTransform().sizeDelta = hh;
    }
    /// <summary>
    /// 不改变宽度适应图片大小
    /// </summary>
    /// <param name="image"></param>
    /// <param name="sprite"></param>
    public static void FitWidth(this Image image, Sprite sprite)
    {
        image.sprite = sprite;
        var hh = image.gameObject.rectTransform().sizeDelta;
        hh.y = (float)sprite.texture.height / sprite.texture.width * hh.x;
        image.gameObject.rectTransform().sizeDelta = hh;
    }
    /// <summary>
    /// 公式编辑器获取计算后的结果
    /// </summary>
    public static double GetExpressionExecuted(this List<FormulaNode> nodes, string rootGUID = "base")
    {
        return Javascript.Eval(GetExpression(nodes, rootGUID));
    }
    /// <summary>
    /// 公式编辑器获取最终式子
    /// </summary>
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

    /// <summary>
    /// 判断两个数误差是否在可接受范围内
    /// </summary>
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

    public static IEnumerator DelayGet(Func<bool> func)
    {
        while (true)
        {
            if (func.Invoke()) break;
            yield return 1;
        }
    }
}
