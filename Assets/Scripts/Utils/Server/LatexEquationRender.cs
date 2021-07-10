/************************************************************************************
    ���ߣ�������
    ������Latex��ʽ��Ⱦ��
*************************************************************************************/
using UnityEngine;
using Flurl.Http;
using System;
using UnityEngine.Events;

public class LatexEquationRender
{
    /// <summary>
    /// ��ȾLatex��Sprite
    /// </summary>
    /// <param name="tex">��ʽ�ַ���</param>
    /// <param name="action">��Ⱦ�ɹ��ص�</param>
    /// <param name="errorHandler">��Ⱦʧ�ܻص�</param>
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
