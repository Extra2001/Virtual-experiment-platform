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

    public GameObject Reason;
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

    private void ClickSureButton()
    {
        string input;
        string answer;
        string reason;

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
        else
        {
            answer = Field.GetComponent<FormulaController>().GetCheckFloat().TrueValue.ToString();
            reason = Field.GetComponent<FormulaController>().CheckError(input);
            try
            {

                answer = Field.GetComponent<FormulaController>().GetCheckFloat().TrueValue.ToString();
                reason = Field.GetComponent<FormulaController>().CheckError(input);
                if (reason != "")
                {
                    Reason.SetActive(true);
                    Reason.GetComponent<Text>().text = reason;
                }
                else
                {
                    Reason.SetActive(false);
                }

                if (input == answer)
                {

                    AnswerText.text = answer;
                    yes.transform.localScale = new Vector3(1, 1, 1);
                    yes.SetActive(true);
                    no.SetActive(false);
                    yes.transform.DOScale(new Vector3(4, 4, 4), 1.5f).OnComplete(() => {
                        yes.SetActive(false);
                    });
                }
                else
                {
                    AnswerText.text = answer;
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
