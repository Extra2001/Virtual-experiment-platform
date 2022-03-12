using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WisdomTree.Common.Function;

public class VSEManager : HTBehaviour
{
    public VSEManager Instance = null;

    public AccountType accountType
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

    public void Init()
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

            if (accountType == AccountType.NotLogin)
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Title = "重要提示",
                    Message = "您未登录账号，仅能体验虚拟实验，存档同步、实验报告上传等功能暂不可用。"
                });
            else if (accountType == AccountType.VSEClass)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Title = "重要提示",
                    Message = "您为微实课堂用户，存档同步暂不可用。"
                });
            }
            else if (accountType == AccountType.Both)
            {
                UIAPI.Instance.ShowModel(new SimpleModel()
                {
                    ShowCancel = false,
                    Title = "重要提示",
                    Message = "您同时登录了智慧树和微实课堂，存档将伴随您的智慧树账户，实验报告将同时上传至两个平台。"
                });
            }
        }
    }

    private void GetAccessToken()
    {

    }

    public enum AccountType
    {
        NotLogin,
        WisdomTree,
        VSEClass,
        Both
    }
}
