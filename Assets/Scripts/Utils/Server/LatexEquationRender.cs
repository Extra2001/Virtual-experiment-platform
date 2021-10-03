/************************************************************************************
    作者：荆煦添
    描述：Latex公式渲染器
*************************************************************************************/
using UnityEngine;
using Flurl.Http;
using System;
using UnityEngine.Events;
using System.IO;
using System.Drawing.Imaging;
using System.Text;

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
        var x = $"http://localhost:{ProcessManager.Port}/".PostJsonAsync(new
        {
            equation = tex
        }).ReceiveString().ContinueWith(xx =>
        {
            byte[] buffers = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    var buffer = Encoding.Default.GetBytes(xx.Result);
                    using (var rms = new MemoryStream(buffer))
                    {
                        var svg = Svg.SvgDocument.Open<Svg.SvgDocument>(rms);
                        var bitmap = svg.Draw(1000, (int)Math.Round((double)svg.Height / svg.Width * 1000));
                        bitmap.Save(ms, ImageFormat.Png);
                        buffers = ms.GetBuffer();
                    }
                }
            }
            catch(Exception ex) { errorHandler?.Invoke(); }
            MainThread.Instance.Run(() =>
            {
                if (buffers != null)
                    action?.Invoke(CommonTools.GetSprite(buffers));
                else errorHandler?.Invoke();
            });
        });
        return;
    }
}
