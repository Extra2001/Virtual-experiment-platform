using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/ModelPanel")]
public class ModelUILogic : UILogicTemporary, IDataDriver<ModelDialogModel>
{
    public ModelDialogModel Data { get; set; } = new ModelDialogModel();

    [DataBinding("Title")] private Text Title;
    [DataBinding("Message")] private Text Message;
    [DataBinding("CancelText")] private Text CancelText;
    [DataBinding("ConfirmText")] private Text ConfirmText;

    private Button CancelButton;
    private Button ConfirmButton;

    /// <summary>
    /// 初始化
    /// </summary>
    public override void OnInit()
    {
        Data = new ModelDialogModel();

        foreach(var item in UIEntity.GetComponentsInChildren<Text>(true))
        {
            if (item.gameObject.name.Equals("Title"))
            {
                Title = item;
            }
            else if (item.gameObject.name.Equals("Message"))
            {
                Message = item;
            }
            else if(item.gameObject.transform.parent.gameObject.name.Equals("ConfirmButton"))
            {
                ConfirmText = item;
                ConfirmButton = item.gameObject.transform.parent.gameObject.GetComponent<Button>();
            }
            else if (item.gameObject.transform.parent.gameObject.name.Equals("CancelButton"))
            {
                CancelText = item;
                CancelButton = item.gameObject.transform.parent.gameObject.GetComponent<Button>();
            }
        }

        base.OnInit();

        CancelButton.onClick.AddListener(() =>
        {
            NavigateBack();
            Data.CancelAction?.Invoke();
        });
        ConfirmButton.onClick.AddListener(() =>
        {
            NavigateBack();
            Data.ConfirmAction?.Invoke();
        });
    }

    public void ShowModel(ModelDialogModel model)
    {
        this.Data.CancelText.Value = model.CancelText.Value;
        this.Data.ConfirmText.Value = model.ConfirmText.Value;
        this.Data.Title.Value = model.Title.Value;
        this.Data.Message.Value = model.Message.Value;

        this.Data.ConfirmAction = model.ConfirmAction;
        this.Data.CancelAction = model.CancelAction;

        Main.m_UI.OpenTemporaryUI<ModelUILogic>();
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);

        UIShowHideHelper.ShowUpToDown(UIEntity);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public override void OnClose()
    {
        base.OnClose();
    }

    /// <summary>
    /// 销毁UI
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// UI逻辑刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}