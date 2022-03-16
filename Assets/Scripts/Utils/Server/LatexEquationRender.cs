/************************************************************************************
    ×÷Õß£º¾£ìãÌí
    ÃèÊö£ºLatex¹«Ê½äÖÈ¾Æ÷
*************************************************************************************/
using UnityEngine;
using System;
using UnityEngine.Events;
using System.IO;
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR
//using System.Drawing.Imaging;
//#endif
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.Networking;
using Unity.VectorGraphics;
using System.Collections;
using Newtonsoft.Json;

public class LatexEquationRender : MonoBehaviour
{
    public static LatexEquationRender Instance = null;
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
            if (base64.Contains("´íÎó"))
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
        tex = tex.Replace("\\", "\\\\");
        string postbody = "{\"auth\":{\"user\":\"guest\",\"password\":\"guest\"},\"latex\":\"" + tex + "\",\"resolution\":600,\"color\":\"000000\"}";
        VSEManager.Instance.StartCoroutine(StartRequest());

        IEnumerator StartRequest()
        {
            using (UnityWebRequest www = UnityWebRequest.Post($"https://latex2png.com/api/convert", postbody))
            {
                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postbody));
                www.uploadHandler.contentType = "application/json";
                www.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                    errorHandler?.Invoke();
                else
                {
                    var text = www.downloadHandler.text;
                    var url = "https://latex2png.com" + JsonConvert.DeserializeObject<RequestModel2>(text).url;
                    using (UnityWebRequest www2 = UnityWebRequest.Get(url))
                    {
                        yield return www2.SendWebRequest();
                        if (www2.result == UnityWebRequest.Result.ConnectionError || www2.result == UnityWebRequest.Result.ProtocolError)
                            errorHandler?.Invoke();
                        var sprite = CommonTools.GetSprite(www2.downloadHandler.data);
                        action?.Invoke(sprite);

                    }
                }
            }
        }
#endif
    }
    public class Model
    {
        public string tex;
        public UnityAction<Sprite> action;
        public UnityAction errorHandler;
    }

    public class RequestModel2
    {
        public string url { get; set; }
    }
}
