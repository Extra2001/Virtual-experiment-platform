using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using DG.Tweening;
/// <summary>
/// 新建流程
/// </summary>
public class EnterClassroomProcedure : ProcedureBase
{
    /// <summary>
    /// 流程初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();
    }

    /// <summary>
    /// 进入流程
    /// </summary>
    /// <param name="lastProcedure">上一个离开的流程</param>
    public override void OnEnter(ProcedureBase lastProcedure)
    {
        /*var prefabInfo = new PrefabInfo(null, null, "Environment/classroom");

        Main.m_Resource.LoadPrefab(prefabInfo, GameObject.Find("GameObject").transform,  loadDoneAction: (obj) =>
        {
            //obj.transform.localScale = new Vector3
            obj.SetActive(true);
        });*/

        base.OnEnter(lastProcedure);
    }

    /// <summary>
    /// 离开流程
    /// </summary>
    /// <param name="nextProcedure">下一个进入的流程</param>
    public override void OnLeave(ProcedureBase nextProcedure)
    {
        base.OnLeave(nextProcedure);
    }

    /// <summary>
    /// 流程帧刷新
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// 流程帧刷新（秒）
    /// </summary>
    public override void OnUpdateSecond()
    {
        base.OnUpdateSecond();
    }
}