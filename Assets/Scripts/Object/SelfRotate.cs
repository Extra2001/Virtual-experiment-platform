using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTEditor;

public class SelfRotate : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    GameObject RTF;

    // Start is called before the first frame update
    void Start()
    {
        RTF = GameObject.Find("(Singleton)RTEditor.EditorObjectSelection");
    }

    // Update is called once per frame
    void Update()
    {
        //if (RTF.GetComponent<EditorObjectSelection>().Skode_Press() == 0)
        //{
        //    this.transform.DOLocalRotate(new Vector3(0, 0, 0), 5f).SetEase(Ease.OutExpo);
        //}
       
    }
}
