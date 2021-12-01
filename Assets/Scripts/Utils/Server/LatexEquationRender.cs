/************************************************************************************
    作者：荆煦添
    描述：Latex公式渲染器
*************************************************************************************/
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Drawing.Imaging;
#endif
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LatexEquationRender : MonoBehaviour
{
    LatexEquationRender Instance = null;
    static List<Model> stack = new List<Model>();
    int callback = 0;
    string base64 = "";
    [DllImport("__Internal")]
    private static extern void Render(string latex, string callbackMono, string callbackFuncName);
    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        if (callback == 2)
        {
            Debug.Log(base64);
            if (base64.Contains("错误"))
                stack[0].errorHandler.Invoke();
            else
                stack[0].action.Invoke(CommonTools.GetSprite(Convert.FromBase64String(base64)));
            stack.RemoveAt(0);
            callback = 0;
        }
        if (callback == 0 && stack.Count > 0)
        {
            Render(stack[0].tex, nameof(LatexEquationRender), nameof(Callback));
            callback = 1;
        }
    }
    public void Callback(string base64)
    {
        callback = 2;
        Debug.Log(base64);
        this.base64 = base64.Replace("data:image/png;base64,", "");
    }
    public static void Render(string tex, UnityAction<Sprite> action = null, UnityAction errorHandler = null)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        stack.Add(new Model()
        {
            tex = tex,
            action = action,
            errorHandler = errorHandler
        });
#endif
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        string postbody = "{\"equation\":\"" + tex + "\"}";
        byte[] postData = Encoding.UTF8.GetBytes(postbody); // 把字符串转换为bype数组
        var www = new UnityWebRequest($"http://localhost:{ProcessManager.Port}/", UnityWebRequest.kHttpVerbPOST);
        www.uploadHandler = new UploadHandlerRaw(postData);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.uploadHandler.contentType = "application/json";
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "application/json");
        www.SendWebRequest().completed += x =>
          {
              if (www.responseCode == 200)
              {
                  byte[] buffers = null;
                  try
                  {
                      using (var ms = new MemoryStream())
                      {
                          var buffer = Encoding.Default.GetBytes(www.downloadHandler.text);
                          using (var rms = new MemoryStream(buffer))
                          {
                              var svg = Svg.SvgDocument.Open<Svg.SvgDocument>(rms);
                              var bitmap = svg.Draw(1000, (int)Math.Round((double)svg.Height / svg.Width * 1000));
                              bitmap.Save(ms, ImageFormat.Png);
                              buffers = ms.GetBuffer();
                          }
                      }
                  }
                  catch { errorHandler?.Invoke(); }
                  if (buffers != null)
                      action?.Invoke(CommonTools.GetSprite(buffers));
                  else errorHandler?.Invoke();
              }
              else
              {
                  errorHandler?.Invoke();
              }
          };
#endif
    }
    public class Model
    {
        public string tex;
        public UnityAction<Sprite> action;
        public UnityAction errorHandler;
    }
}
