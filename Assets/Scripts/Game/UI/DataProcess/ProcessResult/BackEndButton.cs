using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackEndButton : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    public Button BackButton;
    public Button EndButton;

    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(Back);
        EndButton.onClick.AddListener(End);
    }

    public void Back()
    {
        GameManager.Instance.SwitchBackProcedure();
    }
    public void End()
    {
        
    }
}
