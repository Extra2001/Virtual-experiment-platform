using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WisdomTree.Common.Function;
using UnityEngine.Networking;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System;

public class VSEManager : HTBehaviour
{
    public static VSEManager Instance = null;

    private static string appid = "";
    private static string secret = "";
    private static string access_token
    {
        get => PlayerPrefs.HasKey("access_token") ? PlayerPrefs.GetString("access_token") : "";
        set => PlayerPrefs.SetString("access_token", value);
    }
    private static VSEAccessTokenGET vse_user_info
    {
        get => PlayerPrefs.HasKey("vse_user_info") ? JsonConvert.DeserializeObject<VSEAccessTokenGET>(PlayerPrefs.GetString("vse_user_info")) : new VSEAccessTokenGET();
        set => PlayerPrefs.SetString("vse_user_info", JsonConvert.SerializeObject(value));
    }

    public static AccountType accountType
    {
        get
        {
            if (string.IsNullOrEmpty(Communication.ticket) && string.IsNullOrEmpty(Communication._UUID))
                return AccountType.NotLogin;
            if (string.IsNullOrEmpty(Communication.ticket) && (!string.IsNullOrEmpty(Communication._UUID)))
                return AccountType.WisdomTree;
            if ((!string.IsNullOrEmpty(Communication.ticket)) && string.IsNullOrEmpty(Communication._UUID))
                return AccountType.VSEClass;
            return AccountType.Both;
        }
    }

    public static void Init()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<VSEManager>();
        }
        if (Instance == null)
        {
            GameObject communicationObj = new GameObject("_VSEManager");
            DontDestroyOnLoad(communicationObj);
            Instance = communicationObj.AddComponent<VSEManager>();
        }

        Instance.StartCoroutine(WaitForParam());

        IEnumerator WaitForParam()
        {
            yield return new WaitWhile(() => Communication.ticket == "Defult");
            yield return new WaitWhile(() => FindObjectsOfType<SimpleModelPanel>(true).Length == 0);
            Debug.Log(accountType);
            if (accountType == AccountType.NotLogin)
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("重要提示"),
                    Message = new BindableString("您未登录账号，仅能体验虚拟实验，存档同步、实验报告上传等功能暂不可用。")
                });
            else if (accountType == AccountType.VSEClass)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("重要提示"),
                    Message = new BindableString("您为微实课堂用户，存档同步暂不可用。")
                });
                Instance.StartCoroutine(GetAccessToken());
            }
            else if (accountType == AccountType.Both)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("重要提示"),
                    Message = new BindableString("您同时登录了智慧树和微实课堂，存档将伴随您的智慧树账户，实验报告将同时上传至两个平台。")
                });
                Instance.StartCoroutine(GetAccessToken());
            }
        }
    }

    private static IEnumerator GetAccessToken()
    {
        var ticket = Communication.ticket;
        using (var www = UnityWebRequest.Get($"http://39.106.55.217:9099" +
            $"/api/open/token?appid={appid}&ticket={ticket}&signature={GetMd5(ticket + appid + secret)}"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || 
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var text = Encoding.UTF8.GetString(www.downloadHandler.data);
                var model = JsonConvert.DeserializeObject<VSEAccessTokenGET>(text);
                if (model.code == 0)
                {
                    vse_user_info = model;
                    access_token = model.access_token;
                }
                else
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "重要提示",
                        Message = "您微实平台的用户登录失效，请重新从微实平台进入本实验。"
                    });
                }
            }
        }
    }

    public static void UploadReport(VSEReportUpload reportUpload, Action<string> responseAction)
    {
        if (accountType == AccountType.VSEClass || accountType == AccountType.Both)
        {
            reportUpload.appid = appid;
            reportUpload.username = vse_user_info.un;
            string json = JsonConvert.SerializeObject(reportUpload);
            Debug.Log(json);

            Instance.StartCoroutine(StartUploadReport(json, p =>
            {
                try
                {
                    responseAction?.Invoke("0");
                }
                catch
                {
                    responseAction?.Invoke("-1");
                }
            }));
        }

        IEnumerator StartUploadReport(string reportJson, Action<string> responseActionReport)
        {
            using (UnityWebRequest www = UnityWebRequest.Post($"http://39.106.55.217:9099" +
                $"/api/open/dataupload?access_token={access_token}", reportJson))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
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

    private static IEnumerator RefreshAccessToken()
    {
        using (var www = UnityWebRequest.Get($"http://39.106.55.217:9099" +
               $"/api/open/token/refresh?access_token={access_token}&appid={appid}&signature={GetMd5(access_token + appid + secret)}"))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var text = Encoding.UTF8.GetString(www.downloadHandler.data);
                var model = JsonConvert.DeserializeObject<VSEAccessTokenREFRESH>(text);
                if (model.code == 0)
                {
                    access_token = model.access_token;
                }
                else
                {
                    UIAPI.Instance.ShowModel(new SimpleModel()
                    {
                        ShowCancel = false,
                        Title = "重要提示",
                        Message = "您微实平台的用户登录失效，请重新从微实平台进入本实验。"
                    });
                }
            }
        }
    }

    private static string GetMd5(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
            sBuilder.Append(data[i].ToString("x2"));
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public enum AccountType
    {
        NotLogin,
        WisdomTree,
        VSEClass,
        Both
    }
}
