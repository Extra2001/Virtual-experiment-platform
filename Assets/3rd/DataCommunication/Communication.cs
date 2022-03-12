/*****************************************************
* 版权声明：上海卓越睿新数码科技有限公司，保留所有版权
* 文件名称：Communication.cs
* 文件版本：1.0
* 创建时间：2020/06/15 04:29:47
* 作者名称：WangGuanNan
* 文件描述：

*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Linq;

namespace WisdomTree.Common.Function
{
    /// <summary>
    /// 数据通信
    /// </summary>
    public partial class Communication : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void _SendHtml(string key, string param);

        public static void SendHtml(string key, string param)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                _SendHtml(key, param);
            }
            else
            {
                Debug.LogWarning($"当前平台调用SendHtml方法(key:{key}, param:{param})不可用");
            }
        }

        private static readonly string urlGetTime = "https://studyservice.zhihuishu.com/api/stuExperiment/systemTime";

        private static readonly string urlReportFile = "https://newbase.zhihuishu.com/upload/commonUploadFile";

        private static readonly string urlReportImage = "https://virtualcourse.zhihuishu.com/report/uploadReportImage";

        private static readonly string urlReport = "https://virtualcourse.zhihuishu.com/report/saveReport";

        private static readonly string urlNonePicture = "https://image.zhihuishu.com/zhs_yufa_150820/onlineexam/reportImage/202005/d98ad20c32ac40f6b2d78e46ae810bf9.png";

        private static readonly string urlOpenReport = "https://www.zhihuishu.com/virtual_portals_h5/virtualExperiment.html#/experReport/0/0/1/0/";

        public static Communication Instance;

        public static string _UUID = "Defult";

        private static long startTime;

        private static int uploadImageTimes = 3;

        private static string token = "Defult";

        private static Action<Files> audioCallback;
        private static Action<bool> microphoneStateaCallback;
        private static Action<float> audioPlayCallback;

        private static int experimentId;
        private static int courseId;
        private static string appId;
        private static string secret;
        public static string ticket = "Defult";

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(Action callback = null)
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<Communication>();
            }
            if (Instance == null)
            {
                GameObject communicationObj = new GameObject("_Communication");
                DontDestroyOnLoad(communicationObj);
                Instance = communicationObj.AddComponent<Communication>();
            }


            SendHtml("getUrls", "urls");

            SendHtml("getCookie", "uuid");
            SendHtml("getToken", "token");
            SendHtml("getExperimentId", "experimentId");
            SendHtml("getCourseId", "courseId");
            SendHtml("getAppId", "appId");
            SendHtml("getSecret", "secret");
            SendHtml("getTicket", "ticket");

#if UNITY_EDITOR
            experimentId = 371;
            courseId = 2000074157;
            _UUID = "Vj1KNxJG";
            //_UUID = BuilderSettings.GetString("uuid");
            //courseId = BuilderSettings.GetInt("courseId");
            //experimentId = BuilderSettings.GetInt("experimentId");  
            //appId = BuilderSettings.GetString("appId");
            //secret = BuilderSettings.GetString("secret");
            //ticket = BuilderSettings.GetString("ticket");
#endif
            Instance.StartCoroutine(WaitForParam());

            IEnumerator WaitForParam()
            {
                yield return new WaitWhile(() => _UUID == "Defult");

                callback?.Invoke();
            }
        }

        /// <summary>
        /// 获取实验信息
        /// </summary>
        /// <returns></returns>
        public static ExperimentalInformation GetInformation()
        {
            return new ExperimentalInformation()
            {
                uuid = _UUID,
                experimentId = experimentId,
                courseId = courseId
            };
        }

        public static void Eval(string script)
        {
            SendHtml("eval", script);
        }

        public static void SelectLevel(string level)
        {
            SendHtml("selectLevel", level);
        }

        public static void DownloadImage(string urls, Action<Texture2D> action)
        {
            WWWForm form = new WWWForm();
            form.AddField("uuid", _UUID);
            //Debug.Log("uuid:"+ _UUID);
            form.AddField("experimentId", experimentId);
            form.AddField("courseId", courseId);
            //form.AddField("fileKey", DataManager.myselfArtList);
            Instance.StartCoroutine(TextureGet(urls, action));
            IEnumerator TextureGet(string url, Action<Texture2D> act)
            {
                using (UnityWebRequest www = UnityWebRequest.Get(url))
                //using (UnityWebRequest www = UnityWebRequest.Post(url, form))
                {
                    yield return www.SendWebRequest();
                    Debug.Log(url);
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Texture2D tex = new Texture2D(1, 1);
                        tex.LoadImage(www.downloadHandler.data);
                        //art.icon = Sprite.Create(tex, new Rect(Vector2.zero, new Vector2(tex.width, tex.height)), Vector2.zero);
                        Debug.Log(www.downloadHandler.text);
                        act?.Invoke(tex);
                    }
                }
            }
        }

        /// <summary>
        /// 获取时间（服务器获取失败则取本地时间）
        /// </summary>
        /// <param name="responseAction">当前时间</param>
        public static void GetTime(Action<string> responseAction, string format = "yyyy/MM/dd HH:mm:ss")
        {
            Instance.StartCoroutine(_GetTime());

            IEnumerator _GetTime()
            {
                using (UnityWebRequest www = UnityWebRequest.Get(urlGetTime))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        responseAction?.Invoke(DateTime.Now.ToString(format));
                    }
                    else
                    {
                        JObject jObject = JObject.Parse(www.downloadHandler.text);
                        responseAction?.Invoke(ConvertStringToDateTime(jObject["data"].ToObject<long>()).ToString(format));
                    }
                }
            }
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="responseAction"></param>
        public static void GetTimestamp(Action<long> responseAction)
        {
            Instance.StartCoroutine(_GetTime());
            IEnumerator _GetTime()
            {
                using (UnityWebRequest www = UnityWebRequest.Get(urlGetTime))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        responseAction?.Invoke(DateTimeToTimestamp(DateTime.Now));
                    }
                    else
                    {
                        JObject jObject = JObject.Parse(www.downloadHandler.text);

                        responseAction?.Invoke(jObject["data"].ToObject<long>());
                    }
                }
            }
        }

        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long DateTimeToTimestamp(DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (long)(dateTime - startTime).TotalMilliseconds;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="texture">图片</param>
        /// <param name="tryTime">上传尝试次数</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadImage(Texture2D texture, Action<string> responseAction)
        {
            int timeUploadPicture = 0;
            Instance.StartCoroutine(UploadTexture());

            /// <summary>
            /// 上传图片
            /// </summary>
            /// <param name="url">url</param>
            /// <param name="uuid">uuid</param>
            /// <param name="texture">图片</param>
            /// <param name="responseAction">响应事件</param>
            /// <returns></returns>
            IEnumerator UploadTexture()
            {
                UnityWebRequest webRequest = new UnityWebRequest(urlReportImage, "POST");
                WWWForm form = new WWWForm();
                form.AddField("uuid", _UUID);
                form.AddBinaryData("imageFile", texture.EncodeToPNG());

                using (UnityWebRequest www = UnityWebRequest.Post(urlReportImage, form))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        Debug.Log("重新上传");
                        if (timeUploadPicture >= uploadImageTimes)
                        {
                            Debug.LogError("图片 " + texture.name + " 上传失败！");
                            timeUploadPicture = 0;

                            responseAction.Invoke("失败");
                        }
                        else
                        {
                            Instance.StartCoroutine(UploadTexture());
                        }
                        timeUploadPicture++;
                    }
                    else
                    {
                        Debug.Log("图片 " + texture.name + " 上传完成!");
                        Debug.Log(www.downloadHandler.text);
                        responseAction.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="texture">图片</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadFile(byte[] datas, string fileName, Action<string> responseAction)
        {
            int timeUploadPicture = 0;
            Instance.StartCoroutine(UploadFile());

            /// <summary>
            /// 上传图片
            /// </summary>
            /// <param name="url">url</param>
            /// <param name="uuid">uuid</param>
            /// <param name="texture">图片</param>
            /// <param name="responseAction">响应事件</param>
            /// <returns></returns>
            IEnumerator UploadFile()
            {
                WWWForm form = new WWWForm();
                form.AddBinaryData("file", datas, fileName);

                using (UnityWebRequest www = UnityWebRequest.Post(urlReportFile, form))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        Debug.Log("重新上传");
                        if (timeUploadPicture >= uploadImageTimes)
                        {
                            Debug.LogError("文件 " + fileName + " 上传失败！");
                            timeUploadPicture = 0;

                            responseAction.Invoke("失败");
                        }
                        else
                        {
                            Instance.StartCoroutine(UploadFile());
                        }
                        timeUploadPicture++;
                    }
                    else
                    {
                        Debug.Log("文件 " + fileName + " 上传完成!");
                        Debug.Log(www.downloadHandler.text);
                        responseAction.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }

        /// <summary>
        /// 最基本的上传
        /// </summary>
        /// <param name="experimentReport">实验报告</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadReport(ExperimentReport experimentReport, Action<string> responseAction = null)
        {
            string json = JsonConvert.SerializeObject(experimentReport);
            Debug.Log(json);

            Instance.StartCoroutine(StartUploadReport(json, p =>
            {
                try
                {
                    JObject back = JObject.Parse(p);
                    responseAction?.Invoke(urlOpenReport + experimentReport.courseId + "/0?reportId=" + back["reportId"].ToString());
                }
                catch
                {
                    responseAction("-1");
                }
            }));

            IEnumerator StartUploadReport(string reportJson, Action<string> responseActionReport)
            {
                UnityWebRequest webRequest = new UnityWebRequest(urlReport, "POST");
                WWWForm form = new WWWForm();
                form.AddField("jsonStr", reportJson);
                form.AddField("ticket", ticket);

                using (UnityWebRequest www = UnityWebRequest.Post(urlReport, form))
                {
                    yield return www.SendWebRequest();
                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.error);
                        responseActionReport("-1");
                    }
                    else
                    {
                        Debug.Log("实验报告上传完毕!");
                        responseActionReport?.Invoke(www.downloadHandler.text);
                    }
                }
            }
        }

        /// <summary>
        /// 基础上传方法
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="experimentId"></param>
        /// <param name="courseId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="score"></param>
        /// <param name="ReportSummary"></param>
        /// <param name="reportModels"></param>
        /// <param name="responseAction"></param>
        /// <param name="status"></param>
        private static void UploadReport(string uuid, int experimentId, int courseId, long startTime, long endTime, double score, string ReportSummary, ExperimentReportModelBuilder[] reportModels, Step[] steps, Action<string> responseAction = null, int status = 1)
        {
            int timeUsed = (int)((endTime - startTime) * 0.001f);

            appId = ticket == string.Empty ? string.Empty : appId;
            secret = ticket == string.Empty ? string.Empty : secret;

            ExperimentReport.Create(uuid, experimentId, courseId, startTime, endTime, timeUsed, score, ReportSummary, reportModels, report =>
            {
                UploadReport(report, responseAction);
            }, status, steps, appId, secret);
        }

        /// <summary>
        /// 花式上传的基本方法
        /// </summary>
        /// <param name="startTime">实验开始时间</param>
        /// <param name="endTime">实验结束时间</param>
        /// <param name="score">分数</param>
        /// <param name="ReportSummary">实验报告总结</param>
        /// <param name="steps">步骤</param>
        /// <param name="reportModels">模块</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadReport(DateTime startTime, DateTime endTime, double score, string ReportSummary, ExperimentReportModelBuilder[] reportModels, Step[] steps, Action<string> responseAction = null, int status = 1)
        {
            long startTimestamp = DateTimeToTimestamp(startTime);
            long endTimestamp = DateTimeToTimestamp(endTime);

            UploadReport(_UUID, experimentId, courseId, startTimestamp, endTimestamp, score, ReportSummary, reportModels, steps, responseAction, status);
        }

        /// <summary>
        /// 最简上传
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="responseAction">响应事件</param>
        public static void UploadReport(double score, Action<string> responseAction = null, int status = 1)
        {
            GetTimestamp(endTime =>
            {
                UploadReport(_UUID, experimentId, courseId, startTime, endTime, score, "", new ExperimentReportModelBuilder[0], new Step[0], responseAction, status);
            });
        }

        /// <summary>
        /// 花式上传
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="ReportSummary">实验报告总结</param>
        /// <param name="responseAction">响应事件</param>
        /// <param name="reportModels">模块</param>
        public static void UploadReport(double score, string ReportSummary, Action<string> responseAction, params ExperimentReportModelBuilder[] reportModels)
        {
            GetTimestamp(endTime =>
            {
                UploadReport(_UUID, experimentId, courseId, startTime, endTime, score, ReportSummary, reportModels, new Step[0], responseAction);
            });
        }

        /// <summary>
        /// 花式上传
        /// </summary>
        /// <param name="score">分数</param>
        /// <param name="ReportSummary">实验报告总结</param>
        /// <param name="responseAction">响应事件</param>
        /// <param name="steps">步骤</param>
        /// <param name="reportModels">模块</param>
        public static void UploadReport(double score, string ReportSummary, Action<string> responseAction, Step[] steps, params ExperimentReportModelBuilder[] reportModels)
        {
            GetTimestamp(endTime =>
            {
                UploadReport(_UUID, experimentId, courseId, startTime, endTime, score, ReportSummary, reportModels, steps, responseAction);
            });
        }

        /// <summary>
        /// 花式上传
        /// </summary>
        /// <param name="experimentId">实验ID</param>
        /// <param name="score">分数</param>
        /// <param name="ReportSummary">实验报告总结</param>
        /// <param name="responseAction">响应事件</param>
        /// <param name="steps">步骤</param>
        /// <param name="reportModels">模块</param>
        public static void UploadReport(int experimentId, double score, string ReportSummary, Action<string> responseAction, Step[] steps, params ExperimentReportModelBuilder[] reportModels)
        {
            GetTimestamp(endTime =>
            {
                UploadReport(_UUID, experimentId, courseId, startTime, endTime, score, ReportSummary, reportModels, steps, responseAction);
            });
        }

        /// <summary>
        /// 打开实验报告
        /// </summary>
        /// <param name="urlReport">报告链接</param>
        /// <param name="isOnBlankPage">是否在新页面打开</param>
        public static void OpenWebReport(string urlReport, bool isOnBlankPage = true)
        {
            Debug.Log($"window.open('{urlReport}, _blank')");
#if UNITY_EDITOR
            Application.OpenURL(urlReport);
#endif
#pragma warning disable CS0618 //类型或成员已过时
            if (isOnBlankPage) Application.ExternalEval($"window.open('{urlReport}& _blank')");
            else Application.ExternalEval($"window.open('{urlReport}')");
#pragma warning restore CS0618 //类型或成员已过时
        }

        /// <summary>
        /// 接收js消息
        /// </summary>
        /// <param name="identifier">标识符</param>
        /// <param name="data">数据</param>
        public static void Receive(string identifier, string data)
        {
            if (identifier == "getCookie")
            {
                _UUID = (data.Split('=')[1] == null || data.Split('=')[1].StartsWith("undefined")) ? "" : data.Split('=')[1];
                print("uuid设置为" + _UUID);
            }
            else if (identifier == "getToken")
            {
                token = (data.Split('=')[1] == null || data.Split('=')[1].StartsWith("undefined")) ? "" : data.Replace(data.Split('=')[0] + "=", "");
                print("token设置为" + token);
            }
            else if (identifier == "getAudio")
            {
                string audioData = data.Split('=')[1] == null ? "" : data.Split('=')[1];
                Debug.Log(audioData);

                try
                {
                    JObject jObject = JObject.Parse(audioData);
                    Files datas = new Files()
                    {
                        url = jObject["url"].ToString(),
                        type = jObject["type"].ToObject<int>(),
                        fileID = jObject["fileId"].ToObject<int>()
                    };
                    audioCallback?.Invoke(datas);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            else if (identifier == "microphoneState")
            {
                string state = (data.Split('=')[1] == null || data.Split('=')[1].StartsWith("undefined")) ? "0" : data.Split('=')[1];
                microphoneStateaCallback?.Invoke(state == "1");
            }
            else if (identifier == "playAudio")
            {
                audioPlayCallback?.Invoke(float.Parse(data.Split('=')[1]));
            }
            else if (identifier == "getExperimentId")
            {
                experimentId = int.Parse(data.Split('=')[1]);
                print("experimentId设置为" + experimentId);
            }
            else if (identifier == "getCourseId")
            {
                courseId = int.Parse(data.Split('=')[1]);
                print("courseId设置为" + courseId);
            }
            else if (identifier == "getAppId")
            {
                appId = data.Split('=')[1];
                print("appId设置为" + appId);
            }
            else if (identifier == "getSecret")
            {
                secret = data.Replace(data.Split('=')[0] + "=", "");
                print("secret设置为" + secret);
            }
            else if (identifier == "getTicket")
            {
                ticket = data.Replace(data.Split('=')[0] + "=", "");
                print("ticket设置为" + ticket);
            }
            else if (identifier == "getUrls")
            {
                Debug.Log(data);
                string urls = data.Replace(data.Split('=')[0] + "=", "");

                if (urls != "")
                {
                    ChangeVariable(urls);
                }

                GetTimestamp(p => startTime = p);
            }
        }

        private void Receive(string data)
        {
            int index = data.IndexOf('_');
            string left = data.Substring(0, index);
            string right = data.Substring(index + 1, data.Length - index - 1);
            Receive(left, right);
        }

        private static void ChangeVariable(string config)
        {
            JObject jObject = JObject.Parse(config);
            Dictionary<string, string> dic = jObject.ToObject<Dictionary<string, string>>();

            foreach (var item in dic)
            {
                if (item.Value == "") continue;
                Instance.GetType().GetField(item.Key, BindingFlags.Static | BindingFlags.NonPublic).SetValue(Instance, item.Value);
            }
        }
    }

    /// <summary>
    /// 实验报告
    /// </summary>
    public class ExperimentReport
    {
        /// <summary>
        /// 登陆验证的时候会获取到
        /// </summary>
        public string uuid;
        /// <summary>
        /// 实验ID
        /// </summary>
        public int experimentId;
        /// <summary>
        /// 课程ID
        /// </summary>
        public int courseId;
        /// <summary>
        /// 实验开始时间，时间戳
        /// </summary>
        public long startTime;
        /// <summary>
        /// 实验结束时间，时间戳
        /// </summary>
        public long endTime;
        /// <summary>
        /// 实验用时（单位秒）
        /// </summary>
        public int timeUsed;
        /// <summary>
        /// 实验结果（1 - 完成；2 - 未完成）
        /// </summary>
        public int status;
        /// <summary>
        /// 分数
        /// </summary>
        public double score;
        /// <summary>
        /// 由“实验空间”分配给实验教学项目的编号
        /// </summary>
        public string appid = "";
        /// <summary>
        /// 
        /// </summary>
        public string secret = "";
        /// <summary>
        /// 步骤详情
        /// </summary>
        public Step[] steps = new Step[0];
        /// <summary>
        /// 实验报告总结
        /// </summary>
        public string ReportSummary;
        /// <summary>
        /// 实验报告模块
        /// </summary>
        public ExperimentReportModel[] reportModels = new ExperimentReportModel[0];

        public ExperimentReport() { }

        public static void Create(string uuid, int experimentId, int courseId, long startTime, long endTime, int timeUsed, double score, string ReportSummary, ExperimentReportModelBuilder[] reportModels, Action<ExperimentReport> endAction, int status, Step[] steps, string appid, string secret)
        {
            for (int i = 0; i < steps.Length; i++)
            {
                if (steps[i].StepJudge() == false)
                {
                    return;
                }

                if (i + 1 < steps.Length && steps[i].endTime >= steps[i + 1].startTime)
                {
                    Debug.LogWarning($"步骤{steps[i].seq}的结束时间({steps[i].endTime})大于等于步骤{steps[i + 1].seq}的开始时间({steps[i + 1].startTime})");
                }
            }

            if (steps.Length > 0 && steps[0].startTime <= startTime)
            {
                Debug.LogWarning($"步骤{steps[0].seq}的开始时间({steps[0].startTime})小于等于实验的开始时间({startTime})");
            }
            if (steps.Length > 0 && steps[steps.Length - 1].endTime >= endTime)
            {
                Debug.LogWarning($"步骤{steps[steps.Length - 1].seq}的结束时间({steps[steps.Length - 1].endTime})大于等于实验的结束时间({endTime})");
            }

            Communication.Instance.StartCoroutine(Continue());
            IEnumerator Continue()
            {
                List<ExperimentReportModel> models = new List<ExperimentReportModel>();
                for (int i = 0; i < reportModels.Length; i++)
                {
                    List<ExperimentReportContent> experimentReportContentForJsons = new List<ExperimentReportContent>();
                    for (int j = 0; j < reportModels[i].contentBuilders.Length; j++)
                    {
                        ExperimentReportContentBuilder content = reportModels[i].contentBuilders[j];
                        yield return Communication.Instance.StartCoroutine(content.ToContent(contentr =>
                        {
                            experimentReportContentForJsons.Add(contentr);
                        }));
                    }
                    models.Add(new ExperimentReportModel() { name = reportModels[i].name, reportContents = experimentReportContentForJsons.ToArray() });
                }
                endAction.Invoke(new ExperimentReport(uuid, experimentId, courseId, startTime, endTime, timeUsed, score, ReportSummary, models.ToArray(), status, steps, appid, secret));
            }
        }

        public ExperimentReport(string uuid, int experimentId, int courseId, long startTime, long endTime, int timeUsed, double score, string ReportSummary, ExperimentReportModel[] reportModels, int status, Step[] steps, string appid, string secret)
        {
            this.uuid = uuid;
            this.experimentId = experimentId;
            this.courseId = courseId;
            this.startTime = startTime;
            this.endTime = endTime;
            this.timeUsed = timeUsed;
            this.status = status;
            this.score = score;
            this.ReportSummary = ReportSummary;
            this.reportModels = reportModels;
            this.steps = steps;
            this.appid = appid;
            this.secret = secret;
        }
    }

    /// <summary>
    /// 一个实验报告的一模块内容
    /// </summary>
    public class ExperimentReportModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string name = "";
        /// <summary>
        /// 实验报告内容
        /// </summary>
        public ExperimentReportContent[] reportContents = new ExperimentReportContent[0];

        public ExperimentReportModel() { }
    }

    /// <summary>
    /// 实验报告内容
    /// </summary>
    public class ExperimentReportContent
    {
        /// <summary>
        /// 图表名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 图片url（解析imagesForUser自动添加，不需要传参）
        /// </summary>
        public string[] images = new string[] { };
        /// <summary>
        /// 图片类型(static 静态图, dynamic 动态图（多张轮播）, none 无图)
        /// </summary>
        public string imageType = "none";
        /// <summary>
        /// 报告类型，为"inputs"或"script"，其中"inputs"和"script"只有一个有内容另一个传空字符串
        /// </summary>
        public string type = "script";
        /// <summary>
        /// 填写参数
        /// </summary>
        public Dictionary<string, double> inputs = new Dictionary<string, double>();
        /// <summary>
        /// 自主编程脚本，需要把脚本字符串中的"都改为'，双引号都改为单引号    （无脚本需求时，默认为文本内容）
        /// </summary>
        public string script = "";

        public Files[] datas = new Files[] { };
    }

    public class ExperimentReportModelBuilder
    {
        public string name;
        public ExperimentReportContentBuilder[] contentBuilders;

        public ExperimentReportModelBuilder(string name, params ExperimentReportContentBuilder[] contentBuilders)
        {
            this.name = name;
            this.contentBuilders = contentBuilders;
        }
    }

    /// <summary>
    /// 实验报告内容
    /// </summary>
    public class ExperimentReportContentBuilder
    {
        /// <summary>
        /// 图片类型
        /// </summary>
        public enum ImageType
        {
            Static,
            Dynamic,
            None
        }

        /// <summary>
        /// 报告类型
        /// </summary>
        public enum Type
        {
            Script,
            Input
        }

        /// <summary>
        /// 图表名称
        /// </summary>
        public string name = "";
        /// <summary>
        /// 图片url（解析imagesForUser自动添加，不需要传参）
        /// </summary>
        public Texture2D[] images = new Texture2D[] { };
        /// <summary>
        /// 图片类型(static 静态图, dynamic 动态图（多张轮播）, none 无图)
        /// </summary>
        public ImageType imageType = ImageType.None;
        /// <summary>
        /// 报告类型，为"inputs"或"script"，其中"inputs"和"script"只有一个有内容另一个传空字符串
        /// </summary>
        public Type type = Type.Script;
        /// <summary>
        /// 填写参数
        /// </summary>
        public Dictionary<string, double> inputs = new Dictionary<string, double>();
        /// <summary>
        /// 自主编程脚本，需要把脚本字符串中的"都改为'，双引号都改为单引号    （无脚本需求时，默认为文本内容）
        /// </summary>
        public string script = "";

        /// <summary>
        /// 文件
        /// </summary>
        public (string name, byte[] data)[] datas = new (string name, byte[] data)[] { }; //("", new byte[] { })
        public Files[] files = new Files[] { };

        public ExperimentReportContentBuilder() { }
        public ExperimentReportContentBuilder(string name, Texture2D image)
        {
            this.name = name;
            this.images = new Texture2D[] { image };
            this.imageType = ImageType.Static;
        }
        public ExperimentReportContentBuilder(string name, Texture2D[] images, ImageType imageType)
        {
            this.name = name;
            this.images = images;
            this.imageType = imageType;
        }
        public ExperimentReportContentBuilder(string name, Dictionary<string, double> inputs)
        {
            this.name = name;
            this.imageType = ImageType.None;
            this.type = Type.Input;
            this.inputs = inputs;
        }
        public ExperimentReportContentBuilder(string name, string script)
        {
            this.name = name;
            this.imageType = ImageType.None;
            this.type = Type.Script;
            this.script = script;
        }
        public ExperimentReportContentBuilder(string name, Texture2D image, string script)
        {
            this.name = name;
            this.images = new Texture2D[] { image };
            this.imageType = ImageType.Static;
            this.type = Type.Script;
            this.script = script;
        }
        public ExperimentReportContentBuilder(string name, Texture2D[] images, ImageType imageType, string script)
        {
            this.name = name;
            this.images = new Texture2D[] { images[0], images[1] };
            this.imageType = imageType;
            this.type = Type.Script;
            this.script = script;
        }
        public ExperimentReportContentBuilder(string name, Texture2D image, Dictionary<string, double> inputs)
        {
            this.name = name;
            this.images = new Texture2D[] { image };
            this.imageType = ImageType.Static;
            this.type = Type.Script;
            this.inputs = inputs;
        }
        public ExperimentReportContentBuilder(string name, Texture2D[] images, ImageType imageType, Dictionary<string, double> inputs)
        {
            this.name = name;
            this.images = images;
            this.imageType = imageType;
            this.type = Type.Input;
            this.inputs = inputs;
        }
        public ExperimentReportContentBuilder(string name, params (string name, byte[] data)[] datas)
        {
            this.name = name;
            this.datas = datas;
        }
        public ExperimentReportContentBuilder(string name, params Files[] files)
        {
            this.name = name;
            this.files = files;
        }

        public IEnumerator ToContent(Action<ExperimentReportContent> endAction)
        {
            #region 图片上传
            Dictionary<int, string> urls = new Dictionary<int, string>();
            List<string> urls2 = new List<string>();
            for (int j = 0; j < images.Length; j++)
            {
                int index = j;
                Communication.UploadImage(images[j], str =>
                {
                    JObject jObject = JObject.Parse(str);
                    urls.Add(index, jObject["imageUrl"].ToString());
                });
            }
            yield return new WaitUntil(() => urls.Count == images.Length);
            for (int j = 0; j < images.Length; j++)
            {
                urls2.Add(urls[j]);
            }
            #endregion

            #region 文件上传
            Dictionary<int, string> urlsFile = new Dictionary<int, string>();
            List<Files> filesTemp = new List<Files>() { };
            int idUrlsFile = 0;
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].name != "")
                {
                    int indexTemp = i;
                    Communication.UploadFile(datas[i].data, datas[i].name, str =>
                    {
                        JObject jObject = JObject.Parse(str);
                        JObject jObject2 = JObject.Parse(jObject["rt"].ToString());
                        filesTemp.Add(new Files(jObject2["fileName"].ToString(), jObject2["url"].ToString(), jObject2["type"].ToObject<int>(), jObject2["fileId"].ToObject<int>(), indexTemp));
                        urlsFile.Add(idUrlsFile, jObject2["url"].ToString());
                        idUrlsFile++;
                    });
                }
            }
            yield return new WaitUntil(() => urlsFile.Count == datas.Length);
            #endregion
            filesTemp.AddRange(files);
            endAction.Invoke(new ExperimentReportContent()
            {
                name = name,
                imageType = imageType.ToString().ToLower(),
                type = type.ToString().ToLower(),
                inputs = inputs,
                script = script,
                images = urls2.ToArray(),
                datas = filesTemp.ToArray()
            });
        }
    }

    /// <summary>
    /// 文件类（用于音频、视频、文本、可预览文件等的上传）
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName = "";
        /// <summary>
        /// 文件链接
        /// </summary>
        public string url = "";
        /// <summary>
        /// 文件类型
        /// </summary>
        public int type = 0;
        /// <summary>
        /// 文件ID
        /// </summary>
        public int fileID = 0;
        /// <summary>
        /// 文件序号
        /// </summary>
        public int fileOrder = 0;

        public Files() { }
        public Files(string fileName, string url, int type, int fileID, int fileOrder) { this.fileName = fileName; this.url = url; this.type = type; this.fileID = fileID; this.fileOrder = fileOrder; }
    }

    public class StudentInformation
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id;
        /// <summary>
        /// 学生ID
        /// </summary>
        public int studentId;
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string studentName;
        /// <summary>
        /// 班级ID
        /// </summary>
        public int classId;
        /// <summary>
        /// 班级名
        /// </summary>
        public string className;
        /// <summary>
        /// 学院ID
        /// </summary>
        public int collegeId;
        /// <summary>
        /// 学院名
        /// </summary>
        public string collegeName;
        /// <summary>
        /// 学校ID
        /// </summary>
        public int schoolId;
        /// <summary>
        /// 学校名
        /// </summary>
        public string schoolName;
        /// <summary>
        /// 
        /// </summary>
        public string code;
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNumber;
        /// <summary>
        /// 课程ID
        /// </summary>
        public int courseId;
        /// <summary>
        /// 学期ID
        /// </summary>
        public int termId;
        /// <summary>
        /// 消息
        /// </summary>
        public string msg;
        /// <summary>
        /// _UUID
        /// </summary>
        public string uuid;
    }

    public class UserInformation
    {
        /// <summary>
        /// 班级ID
        /// </summary>
        public int classId;
        /// <summary>
        /// 学院ID
        /// </summary>
        public int collegeId;
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName;
        /// <summary>
        /// 学号
        /// </summary>
        public string userCode;
    }

    public struct ExperimentalInformation
    {
        public string uuid;
        public int experimentId;
        public int courseId;
    }

    /// <summary>
    /// 步骤详情
    /// </summary>
    public class Step
    {
        /// <summary>
        /// 实验步骤序号
        /// </summary>
        public int seq = 0;

        /// <summary>
        /// 步骤名称（20字以内）
        /// </summary>
        public string title = "";

        /// <summary>
        /// 实验步骤开始时间
        /// </summary>
        public long startTime = 0;

        /// <summary>
        /// 实验步骤结束时间
        /// </summary>
        public long endTime = 0;

        /// <summary>
        /// 实验步骤用时（单位秒）
        /// </summary>
        public int timeUsed = 0;

        /// <summary>
        /// 实验步骤合理用时（单位秒）
        /// </summary>
        public int expectTime = 0;

        /// <summary>
        /// 实验步骤得分（0 ~100，百分制）
        /// </summary>
        public int score = 0;

        /// <summary>
        /// 实验步骤满分（0 ~100，百分制）
        /// </summary>
        public int maxScore = 0;

        /// <summary>
        /// 实验步骤操作次数
        /// </summary>
        public int repeatCount = 1;

        /// <summary>
        /// 步骤评价（200字以内）
        /// </summary>
        public string evaluation = "";

        /// <summary>
        /// 赋分模型（200字以内）
        /// </summary>
        public string scoringModel = "";

        /// <summary>
        /// 备注（200字以内）
        /// </summary>
        public string remarks = "";

        public Step() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="seq">实验步骤序号</param>
        /// <param name="title">步骤名称（20字以内）</param>
        /// <param name="startTime">实验步骤开始时间</param>
        /// <param name="endTime">实验步骤结束时间</param>
        /// <param name="expectTime">实验步骤合理用时（单位秒）</param>
        /// <param name="score">实验步骤得分</param>
        /// <param name="maxScore">实验步骤满分</param>
        /// <param name="repeatCount">实验步骤操作次数</param>
        /// <param name="evaluation">步骤评价</param>
        /// <param name="scoringModel">赋分模型</param>
        /// <param name="remarks">备注</param>
        public Step(int seq, string title, DateTime startTime, DateTime endTime, int expectTime, int score, int maxScore, int repeatCount, string evaluation, string scoringModel, string remarks = "")
        {
            this.seq = seq;
            this.title = title;
            this.startTime = Communication.DateTimeToTimestamp(startTime);
            this.endTime = Communication.DateTimeToTimestamp(endTime);
            this.timeUsed = (int)((this.endTime - this.startTime) * 0.001f);
            this.expectTime = expectTime;
            this.score = score;
            this.maxScore = maxScore;
            this.repeatCount = repeatCount;
            this.evaluation = evaluation;
            this.scoringModel = scoringModel;
            this.remarks = remarks;

            StepJudge();
        }

        public bool StepJudge()
        {
            if (title == "")
            {
                Debug.LogError($"步骤{seq}的“步骤名称(title)”不可为空");
                return false;
            }
            if (evaluation == "")
            {
                Debug.LogError($"步骤{seq}的“步骤评价(evaluation)”不可为空");
                return false;
            }
            if (scoringModel == "")
            {
                Debug.LogError($"步骤{seq}的“赋分模型(scoringModel)”不可为空");
                return false;
            }
            if ((this.endTime - this.startTime) <= 0)
            {
                Debug.LogError($"步骤{seq}用时错误");
                return false;
            }

            return true;
        }
    }
}   