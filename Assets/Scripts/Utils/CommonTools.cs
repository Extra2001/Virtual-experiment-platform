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
            var baseType = itemType.BaseType;
            if (baseType != null)
            {
                if (baseType.Name == parentType.Name)
                {
                    subTypeList.Add(itemType);
                }
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
            Texture2D texture2D = new Texture2D(0, 0);
            texture2D.LoadImage(bytes);

            Sprite ret = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
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
}
