using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
