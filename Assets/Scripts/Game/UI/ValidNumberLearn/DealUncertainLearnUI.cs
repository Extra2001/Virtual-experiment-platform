using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealUncertainLearnUI : HTBehaviour
{
    public Button BackButton;
    public Button[] ChoiceButtons;
    public GameObject HintText;
    public GameObject CalcZone;

    private int CurrentIndex = -1;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(ClickBackButton);
        ChoiceButtons[0].onClick.AddListener(ClickAddAndSubtractButton);
        ChoiceButtons[1].onClick.AddListener(ClickMultiplyAndDivideButton);
        ChoiceButtons[2].onClick.AddListener(ClickFunctionCalcButton);
        ChoiceButtons[3].onClick.AddListener(ClickMixedCalcButton);

        HintText.SetActive(true);
        CalcZone.SetActive(false);
    }

    
    private void SwitchZone(int index)
    {
        HintText.SetActive(false);
        CalcZone.SetActive(true);
        for (int i = 0; i < ChoiceButtons.Length; i++)
        {
            if (i == index)
            {
                ChoiceButtons[i].image.color = new Color(0, 101 / 255f, 195 / 255f, 255 / 255f);
            }
            else
            {
                ChoiceButtons[i].image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            }
        }
    }

    /// <summary>
    /// 选择加减、乘除、函数、混合运算按钮
    /// </summary>
    private void ClickAddAndSubtractButton()
    {
        CurrentIndex = 0;
        SwitchZone(CurrentIndex);
    }
    private void ClickMultiplyAndDivideButton()
    {
        CurrentIndex = 1;
        SwitchZone(CurrentIndex);
    }
    private void ClickFunctionCalcButton()
    {
        CurrentIndex = 2;
        SwitchZone(CurrentIndex);
    }
    private void ClickMixedCalcButton()
    {
        CurrentIndex = 3;
        SwitchZone(CurrentIndex);
    }
    /// <summary>
    /// 点击“返回”按钮
    /// </summary>
    private void ClickBackButton()
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "继续返回将丢失当前进度，继续？",
            ConfirmAction = () =>
            {
                GameManager.Instance.SwitchBackToStart();
            }
        });
    }
}
