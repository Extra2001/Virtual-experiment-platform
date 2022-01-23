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
    public GameObject[] Zones;

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
        if (index == 3)
        {
            for (int i = 0; i < Zones.Length; i++)
            {
                if (i < 3)
                {
                    Zones[i].SetActive(false);
                }
                else
                {
                    Zones[i].SetActive(true);
                }                
            }
        }
        else
        {
            for (int i = 0; i < Zones.Length; i++)
            {
                if (i == index)
                {
                    Zones[i].SetActive(true);
                }
                else
                {
                    Zones[i].SetActive(false);
                }
            }
        }

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
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "加减运算不能分步进行，结果与绝对不确定度最大者的最后一位对齐",
            ShowCancel = false
        });
    }
    private void ClickMultiplyAndDivideButton()
    {
        CurrentIndex = 1;
        SwitchZone(CurrentIndex);
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "乘除运算不能分步进行，以有效数字最少的输入量为准",
            ShowCancel = false
        });
    }
    private void ClickFunctionCalcButton()
    {
        CurrentIndex = 2;
        SwitchZone(CurrentIndex);
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "函数运算利用微分法则确定有效数字位数",
            ShowCancel = false
        });
    }
    private void ClickMixedCalcButton()
    {
        CurrentIndex = 3;
        SwitchZone(CurrentIndex);
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "混合运算分步进行，中间过程多保留1-2位有效数字，在最终结果处截断",
            ShowCancel = false
        });
    }
    /// <summary>
    /// 点击“返回”按钮
    /// </summary>
    private void ClickBackButton()
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Message = "继续返回可能丢失当前进度，继续？",
            ConfirmAction = () =>
            {
                GameManager.Instance.SwitchBackToStart();
            }
        });
    }
}
