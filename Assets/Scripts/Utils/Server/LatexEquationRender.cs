/************************************************************************************
    作者：荆煦添
    描述：Latex公式渲染器
*************************************************************************************/
using UnityEngine;
using Flurl.Http;
using System;
using UnityEngine.Events;

public class LatexEquationRender
{
    /// <summary>
    /// 渲染Latex到Sprite
    /// </summary>
    /// <param name="tex">公式字符串</param>
    /// <param name="action">渲染成功回调</param>
    /// <param name="errorHandler">渲染失败回调</param>
    public static void Render(string tex, UnityAction<Sprite> action = null, UnityAction errorHandler = null)
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
                catch { errorHandler?.Invoke(); }
            });
        });
        return;
    }
}
