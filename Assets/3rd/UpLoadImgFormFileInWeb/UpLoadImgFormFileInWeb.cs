
/*****************************************************
* 版权声明：上海卓越睿新数码科技有限公司，保留所有版权
* 文件名称：UpLoadImgFormFileInWeb.cs
* 文件版本：1.0
* 创建时间：2020/12/25 07:03:09
* 作者名称：Xuxiaohao
* 文件描述：

*****************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WisdomTree.Xuxiaohao.Function
{
    /// <summary>
    /// WebGL InputField Js 引用
    /// </summary>
    public static class UpLoadImgFormFileInWeb
    {
#if UNITY_WEBGL
        /// <summary>
        /// 网页回调时的方法
        /// </summary>
        /// <param name="ReceiveMsgGameObjectName">接收消息名字</param>
        /// <param name="ReceiveMsgMethodName">接收消息方法</param>
        [DllImport("__Internal")]
        public static extern void GetImgFromFile(string ReceiveMsgGameObjectName, string ReceiveMsgMethodName);

      //  public static void GetImgFromFile(string ReceiveMsgGameObjectName, string ReceiveMsgMethodName) { }
#endif
#if UNITY_EDITOR
        public static void GetImgFromFile(string ReceiveMsgGameObjectName, string ReceiveMsgMethodName) { }
#endif
    }
}