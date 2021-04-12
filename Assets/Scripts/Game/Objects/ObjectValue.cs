using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectValue : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public float Length;
    public float Width;
    public float Height;
    public float Mass;

    private void Update()
    {
        //Debug.Log("长度：" + Length);
        //Debug.Log("宽度:" + Width);
        //Debug.Log("高度：" + Height);
        //Debug.Log("质量：" + Mass);
    }
}
