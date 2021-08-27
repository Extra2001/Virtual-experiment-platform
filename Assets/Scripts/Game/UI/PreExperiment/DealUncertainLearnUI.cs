using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealUncertainLearnUI : HTBehaviour
{
    public Button BackButton;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(ClickBackButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClickBackButton()
    {
        UIAPI.Instance.ShowModel(new ModelDialogModel()
        {
            Message = new BindableString("继续返回将丢失当前进度，继续？"),
            ConfirmAction = () =>
            {
                GameManager.Instance.SwitchBackToStart();
            }
        });
    }
}
