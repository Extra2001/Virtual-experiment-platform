using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flurl.Http;
using System.Threading.Tasks;
using System;
using UnityEngine.Events;

public class LatexEquationRender
{
    public static Sprite Render(string tex)
    {
        var x = $"http://expr.buaaer.top/".PostJsonAsync(new
        {
            equation = tex
        }).ReceiveString().Result;
        return CommonTools.GetSprite(Convert.FromBase64String(x.Replace("data:image/png;base64,", "")));
    }

    public static void Render(string tex, UnityAction<Sprite> action)
    {
        var x = $"http://expr.buaaer.top/".PostJsonAsync(new
        {
            equation = tex
        }).ReceiveString().ContinueWith(xx =>
        {
            MainThread.Instance.Run(() =>
            {
                try
                {
                    action?.Invoke(CommonTools.GetSprite(Convert.FromBase64String(xx.Result.Replace("data:image/png;base64,", ""))));
                }
                catch { }
            });
        });
        return;
    }
}
