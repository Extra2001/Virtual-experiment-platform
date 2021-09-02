using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealUncertainLearnUI : HTBehaviour
{
    public Button BackButton;
    public Button SureButton;
    public Text AnswerText;
    public InputField UserInput;
    public GameObject Field;

    public GameObject yes;
    public GameObject no;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        BackButton.onClick.AddListener(ClickBackButton);
        SureButton.onClick.AddListener(ClickSureButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ClickSureButton()
    {
        FormulaController formula;
        string input;
        string answer="0.1";

        try
        {
            //formula = Field.GetComponent<FormulaController>().GetCheckFloat()
            //formula = Field.GetComponent<FormulaController>().Expression;
            CheckFloat temp = Field.GetComponent<FormulaController>().GetCheckFloat();
            answer = temp.TrueValue.ToString();
            AnswerText.text = answer;

            input = UserInput.text;
            if (input == "")
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Title = new BindableString("警告"),
                    Message = new BindableString("请输入你认为的有效数字位数")
                });
            }
            else if (input == answer)
            {
                yes.transform.localScale = new Vector3(1, 1, 1);
                yes.SetActive(true);
                no.SetActive(false);
                yes.transform.DOScale(new Vector3(4, 4, 4), 1.5f).OnComplete(() => {
                    yes.SetActive(false);
                });
            }
            else
            {
                no.transform.localScale = new Vector3(1, 1, 1);
                yes.SetActive(false);
                no.SetActive(true);
                no.transform.DOScale(new Vector3(4, 4, 4), 1.5f).OnComplete(() => {
                    no.SetActive(false);
                });
            }

        }
        catch
        {
            //弹出报错提示框
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                ShowCancel = false,
                Title = new BindableString("错误"),
                Message = new BindableString("输入表达式不合法，请仔细检查")
            });
        }
        

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
