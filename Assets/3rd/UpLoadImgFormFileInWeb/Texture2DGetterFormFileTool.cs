
/*****************************************************
* 版权声明：上海卓越睿新数码科技有限公司，保留所有版权
* 文件名称：Texture2DGetterFormFile.cs
* 文件版本：1.0
* 创建时间：2020/12/27 03:42:33
* 作者名称：Xuxiaohao
* 文件描述：

*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using UniRx.Async;
using UnityEngine.Networking;

namespace WisdomTree.Xuxiaohao.Function
{
    public static partial class ToolHelper
    {
        public static partial class UITool
        {

            public static partial class TextureTool
            {

                #region Webgl
                public static class Texture2DGetterFormFile_Webgl
                {

                    /// <summary>
                    /// 接收网页选中的图片
                    /// </summary>
                    /// <param name="ReceiveMsgGameObjectName">用于回调接收图片方法的Gameobject的名字(尽量唯一)</param>
                    /// <param name="ReceiveMsgMethodName">用于回调接收图片方法名字</param>
                    public static void GetTexture2DFormFile_Webgl(string ReceiveMsgGameObjectName, string ReceiveMsgMethodName)
                    {
                        UpLoadImgFormFileInWeb.GetImgFromFile(ReceiveMsgGameObjectName, ReceiveMsgMethodName);
                    }
                    /// <summary>
                    /// 接收网页选中的图片
                    /// </summary>
                    /// <param name="gameObject">用于回调接收图片方法的Gameobject(名字尽量唯一)</param>
                    /// <param name="recivePngStrMethodInMonoBehaviour">用于回调接收图片方法(必须在gameObject 某个挂载的MonoBehaviour里有对应的方法 不能是匿名)</param>
                    public static void GetTexture2DFormFile_Webgl(GameObject gameObject, Action<string> recivePngStrMethodInMonoBehaviour)
                    {
                        UpLoadImgFormFileInWeb.GetImgFromFile(gameObject.name, recivePngStrMethodInMonoBehaviour.Method.Name);
                    }
                #region Web
                    /// <summary>
                    /// 接收网页传回字符串转成图片方法示例
                    /// </summary>
                    /// <param name="str"></param>
                    public static void ReceiveImgString(string str)
                    {
                        Texture2D texture2D = Texture2DGetterFormFile_Webgl.WebglStringToTextur2D(str);
                        // ToolHelper.UI.TextureTool.
                    }
                    /// <summary>
                    /// 接收网页传回字符串转成图片
                    /// </summary>
                    /// <param name="str"></param>
                    public static Texture2D WebglStringToTextur2D(string str)
                    {
                        string t = str.Split(',')[1];
                        Texture2D t2D = Base64StringToImage(t);
                        return t2D;
                    }
                    /// <summary>
                    /// base64编码的文本 转为Texture2D材质
                    /// </summary>
                    /// <param name="basestr">base64字符串</param>
                    /// <returns>转换后的Bitmap对象</returns>
                    public static Texture2D Base64StringToImage(string basestr)
                    {
                        Texture2D t2d = new Texture2D(1, 1);
                        try
                        {
                            byte[] arr = Convert.FromBase64String(basestr);
                            t2d.LoadImage(arr);
                        }
                        catch (Exception ex)
                        {
                            Debug.Log("转换失败");
                        }
                        return t2d;
                    }
                #endregion
                }
                #endregion

                #region PC
                public class Texture2DGetterFormFile_WinPC
                {
                    public static void UpLodImage(Action<Texture2D> load)
                    {
#if UNITY_EDITOR
                        Debug.Log("UpLodImage begin");
                        #region UniRx
                        //FileWindow.OpenProject("请选择贴图", new FileFilter[] { new FileFilter("图片", "png", "jpg") }, Application.dataPath, async p =>
                        //{
                        //    Texture2D texture2D = await DownloadTexture2D(p);
                        //    if (load != null)
                        //    {
                        //        load(texture2D);
                        //    }
                        //});
                        #endregion
                        /*
                        FileWindow.OpenProject("请选择贴图", new FileFilter[] { new FileFilter("图片", "png", "jpg") }, Application.dataPath, p =>
                        {
                            GameObject gameObject = new GameObject();
                            LoadMono loadMono = gameObject.AddComponent<LoadMono>();
                            loadMono.StartCoroutine(loadMono.Load(p, load));
                        });
                        */
#else
#endif
                        Debug.Log("UpLodImage end");
                    }
                    #region UniRx
                    //public static async UniTask Set(string path, Action<Texture2D> loadAction)
                    //{
                    //    Texture2D texture = await DownloadTexture2D(path);
                    //    if (loadAction != null)
                    //    {
                    //        loadAction(texture);
                    //    }
                    //}

                    ///// <summary>
                    ///// 获取实验配置（实验数据读取）
                    ///// </summary>
                    ///// <param name="experimentId">实验ID</param>
                    ///// <param name="courseId">课程ID</param>
                    ///// <param name="key">文本标识</param>
                    ///// <param name="responseAction">响应事件 </param>
                    //public static async UniTask<Texture2D> DownloadTexture2D(string path)
                    //{
                    //    using (UnityWebRequest www = new UnityWebRequest(path))
                    //    {
                    //         www.downloadHandler = new DownloadHandlerBuffer();
                    //        await www.SendWebRequest();
                    //        if (www.isNetworkError || www.isHttpError)
                    //        {
                    //            Debug.Log(www.error);
                    //            return null;
                    //        }
                    //        else
                    //        {
                    //            Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
                    //            texture.LoadImage(www.downloadHandler.data);
                    //            texture.Apply();
                    //            return texture;
                    //        }
                    //    }
                    //}
                    #endregion
                    public class LoadMono : MonoBehaviour
                    {
                        public IEnumerator Load(string path, System.Action<Texture2D> loadAction)
                        {
                            Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGBA32, false);
                            using (UnityWebRequest unityWebRequest = new UnityWebRequest(path))
                            {                              
                                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                                yield return unityWebRequest.SendWebRequest();
                                texture.LoadImage(unityWebRequest.downloadHandler.data);
                                texture.Apply();
                                loadAction?.Invoke(texture);                     
                                GameObject.DestroyImmediate(gameObject);
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }
}