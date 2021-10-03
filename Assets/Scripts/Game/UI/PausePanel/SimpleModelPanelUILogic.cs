using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建UI逻辑类
/// </summary>
[UIResource(null, null, "UI/SimpleModelPanel")]
public class SimpleModelPanelUILogic : UILogicTemporary
{
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);
    }

    public void ShowModel(SimpleModel model)
    {
        UIEntity.GetComponent<SimpleModelPanel>().ShowModel(model);
    }
}
