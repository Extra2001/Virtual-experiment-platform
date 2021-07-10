/************************************************************************************
    作者：荆煦添
    描述：模态提示框的数据模型，用于显示模态提示窗
*************************************************************************************/
using HT.Framework;
using UnityEngine.Events;

public class ModelDialogModel
{
    public BindableString Title = new BindableString("提示");

    public BindableString Message = new BindableString("");

    public BindableString CancelText = new BindableString("取消");

    public BindableString ConfirmText = new BindableString("确认");

    public UnityAction CancelAction { get; set; }

    public UnityAction ConfirmAction { get; set; }

    public bool ShowCancel { get; set; } = true;
}
