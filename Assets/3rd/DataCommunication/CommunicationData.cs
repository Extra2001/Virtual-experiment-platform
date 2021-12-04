using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json.Linq;

namespace WisdomTree.Common.Function
{
    public partial class Communication
    {
        private static readonly string urlUploadText = "https://virtualcourse.zhihuishu.com/report/saveExperimentText";

        private static readonly string urlGetText = "https://virtualcourse.zhihuishu.com/report/getExperimentText";

        private static readonly string urlUploadConfig = "https://virtualcourse.zhihuishu.com/report/saveExperimentTextNew";

        private static readonly string urlDownloadConfig = "https://virtualcourse.zhihuishu.com/report/getExperimentTextNew";

        private static readonly string urlGetExperimentKeyUser = "https://virtualcourse.zhihuishu.com/report/getExperimentKey";

        private static readonly string urlDeleteExperimentKeyUser = "https://virtualcourse.zhihuishu.com/report/deleteExperimentKey";

        private static readonly string urlGetExperimentKey = "https://virtualcourse.zhihuishu.com/report/getExperimentKeyNew";

        private static readonly string urlDeleteExperimentKey = "https://virtualcourse.zhihuishu.com/report/deleteExperimentKeyNew";

        #region 上传文本
        /// <summary>
        /// 上传文本（实验数据上传保存）
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadText(string key, string data, Action<string> responseAction, Action<string> errorHandler = null)
        {
            UploadText(experimentId, key, data, responseAction, errorHandler);
        }

        /// <summary>
        /// 上传文本（实验数据上传保存）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadText(int experimentId, string key, string data, Action<string> responseAction, Action<string> errorHandler = null)
        {
            UploadText(experimentId, _UUID, key, data, responseAction, errorHandler);
        }

        /// <summary>
        /// 上传文本（实验数据上传保存）
        /// </summary>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadText(string uuid, string key, string data, Action<string> responseAction, Action<string> errorHandler = null)
        {
            UploadText(experimentId, uuid, key, data, responseAction, errorHandler);
        }

        /// <summary>
        /// 上传文本（实验数据上传保存）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadText(int experimentId, string uuid, string key, string data, Action<string> responseAction, Action<string> errorHandler = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("uuid", uuid);
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);
            form.AddField("data", data);

            Instance.StartCoroutine(Upload());

            IEnumerator Upload()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlUploadText, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        errorHandler?.Invoke("请求发生错误");
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);
                        responseAction?.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }
        #endregion

        #region 获取文本
        /// <summary>
        /// 获取文本（实验数据读取）
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadText(string key, Action<string> responseAction, Action<string> errorHandler = null)
        {
            DownloadText(experimentId, _UUID, key, responseAction, errorHandler);
        }

        /// <summary>
        /// 获取文本（实验数据读取）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadText(int experimentId, string key, Action<string> responseAction, Action<string> errorHandler = null)
        {
            DownloadText(experimentId, _UUID, key, responseAction, errorHandler);
        }

        /// <summary>
        /// 获取文本（实验数据读取）
        /// </summary>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadText(string uuid, string key, Action<string> responseAction, Action<string> errorHandler = null)
        {
            DownloadText(experimentId, uuid, key, responseAction, errorHandler);
        }

        /// <summary>
        /// 获取文本（实验数据读取）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadText(int experimentId, string uuid, string key, Action<string> responseAction, Action<string> errorHandler = null)
        {
            WWWForm form = new WWWForm();
            form.AddField("uuid", uuid);
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlGetText, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        errorHandler?.Invoke("请求发生错误");
                    }
                    else
                    {
                        responseAction?.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }
        #endregion

        #region 上传实验配置
        /// <summary>
        /// 上传实验配置（实验数据上传保存）
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadConfig(string key, string data, Action<string> responseAction)
        {
            UploadConfig(experimentId, key, data, responseAction);
        }

        /// <summary>
        /// 上传实验配置（实验数据上传保存）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="data">文本内容</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadConfig(int experimentId, string key, string data, Action<string> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);
            form.AddField("data", data);

            Instance.StartCoroutine(Upload());

            IEnumerator Upload()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlUploadConfig, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);
                        responseAction?.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }
        #endregion

        #region 获取实验配置
        /// <summary>
        /// 获取实验配置（实验数据读取）
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadConfig(string key, Action<string> responseAction)
        {
            DownloadConfig(experimentId, key, responseAction);
        }

        /// <summary>
        /// 获取实验配置（实验数据读取）
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件 </param>
        public static void DownloadConfig(int experimentId, string key, Action<string> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlDownloadConfig, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        if (jObject != null && jObject["data"] != null)
                        {
                            responseAction?.Invoke(jObject["data"].ToString());
                        }
                        else
                        {
                            Debug.Log("error:" + www.downloadHandler.text);
                        }
                    }
                }
            }
        }
        #endregion

        #region 获取用户的所有key
        /// <summary>
        /// 获取用户的所有Key
        /// </summary>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKeyUser(Action<List<string>> responseAction)
        {
            GetExperimentKeyUser(_UUID, responseAction);
        }

        /// <summary>
        /// 获取用户的所有Key
        /// </summary>
        /// <param name="uuid">用户ID</param>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKeyUser(string uuid, Action<List<string>> responseAction)
        {
            GetExperimentKeyUser(experimentId, uuid, responseAction);
        }

        /// <summary>
        /// 获取用户的所有Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKeyUser(int experimentId, Action<List<string>> responseAction)
        {
            GetExperimentKeyUser(experimentId, _UUID, responseAction);
        }

        /// <summary>
        /// 获取用户的所有Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="uuid">用户ID</param>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKeyUser(int experimentId, string uuid, Action<List<string>> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("uuid", uuid);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlGetExperimentKeyUser, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        if (jObject != null && jObject["data"] != null)
                        {
                            responseAction?.Invoke(jObject["data"].ToObject<List<string>>());
                        }
                        else
                        {
                            Debug.Log("error:" + www.downloadHandler.text);
                        }
                    }
                }
            }
        }
        #endregion

        #region 删除用户的key
        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKeyUser(string key, Action<bool> responseAction)
        {
            DeleteExperimentKeyUser(_UUID, key, responseAction);
        }

        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKeyUser(string uuid, string key, Action<bool> responseAction)
        {
            DeleteExperimentKeyUser(experimentId, uuid, key, responseAction);
        }

        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKeyUser(int experimentId, string key, Action<bool> responseAction)
        {
            DeleteExperimentKeyUser(experimentId, _UUID, key, responseAction);
        }

        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="uuid">用户ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKeyUser(int experimentId, string uuid, string key, Action<bool> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);
            form.AddField("uuid", uuid);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlDeleteExperimentKeyUser, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        responseAction?.Invoke(false);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        if (jObject != null)
                        {
                            responseAction?.Invoke(jObject["code"].ToObject<int>() == 0);
                        }
                        else
                        {
                            Debug.Log("error:" + www.downloadHandler.text);
                            responseAction?.Invoke(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region 获取实验配置的所有key
        /// <summary>
        /// 获取该实验的所有Key
        /// </summary>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKey(Action<List<string>> responseAction)
        {
            GetExperimentKey(experimentId, responseAction);
        }

        /// <summary>
        /// 获取该实验的所有Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="responseAction">响应事件 </param>
        public static void GetExperimentKey(int experimentId, Action<List<string>> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlGetExperimentKey, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        if (jObject != null && jObject["data"] != null)
                        {
                            responseAction?.Invoke(jObject["data"].ToObject<List<string>>());
                        }
                        else
                        {
                            Debug.Log("error:" + www.downloadHandler.text);
                        }
                    }
                }
            }
        }
        #endregion

        #region 删除实验的key
        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKey(string key, Action<bool> responseAction)
        {
            DeleteExperimentKey(experimentId, key, responseAction);
        }

        /// <summary>
        /// 删除实验的Key
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="key">文本标识</param>
        /// <param name="responseAction">响应事件</param>
        public static void DeleteExperimentKey(int experimentId, string key, Action<bool> responseAction)
        {
            WWWForm form = new WWWForm();
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            form.AddField("fileKey", key);

            Instance.StartCoroutine(Get());

            IEnumerator Get()
            {
                using (UnityWebRequest www = UnityWebRequest.Post(urlDeleteExperimentKey, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        responseAction?.Invoke(false);
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        if (jObject != null)
                        {
                            responseAction?.Invoke(jObject["code"].ToObject<int>() == 0);
                        }
                        else
                        {
                            Debug.Log("error:" + www.downloadHandler.text);
                            responseAction?.Invoke(false);
                        }
                    }
                }
            }
        }
        #endregion
    }
}