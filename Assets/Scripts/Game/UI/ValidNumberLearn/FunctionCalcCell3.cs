using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionCalcCell3 : HTBehaviour
{
    public InputField[] Angle = new InputField[3];
    public bool[] AngleKind = new bool[3] { true, true, true };
    public Toggle[] Toggles = new Toggle[3];

    public GameObject root;
    public int id;
    public bool[] FinishSituation = new bool[3] { false, false, false }; //0,1,2代表度分秒
    //启用自动化
    protected override bool IsAutomate => true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Toggles.Length; i++)
        {
            Toggles[i].isOn = true;
            int index = i;
            Toggles[i].onValueChanged.AddListener(isOn =>
            {
                AngleKind[index] = isOn;
                root.GetComponent<DealCalc3>().CellValue[id].AngleKind[index] = isOn;
                Angle[index].gameObject.SetActive(isOn);
                FinishSituation[index] = false;
            });
        }

        Angle[0].onEndEdit.AddListener(value =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                root.GetComponent<DealCalc3>().CellValue[id].Angle[0] = value;
                FinishSituation[0] = true;
                return;
            }
            FinishSituation[0] = false;
        });
        Angle[1].onEndEdit.AddListener(value =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                if((int.Parse(value)>=0)&& (int.Parse(value) < 60))
                {
                    root.GetComponent<DealCalc3>().CellValue[id].Angle[1] = value;
                    FinishSituation[1] = true;
                    return;
                }
                else
                {
                    Angle[1].text = string.Empty;
                    WarningInput();
                }
                
            }
            FinishSituation[1] = false;
        });
        Angle[2].onEndEdit.AddListener(value =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                if ((int.Parse(value) >= 0) && (int.Parse(value) < 60))
                {
                    root.GetComponent<DealCalc3>().CellValue[id].Angle[2] = value;
                    FinishSituation[2] = true;
                    return;
                }
                else
                {
                    Angle[2].text = string.Empty;
                    WarningInput();
                }

            }
            FinishSituation[1] = false;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void WarningInput()
    {
        UIAPI.Instance.ShowModel(new SimpleModel()
        {
            Title = "警告",
            Message = "请输入合法的数字",
            ShowCancel = false
        });
    }
    private bool TongHuo(bool a, bool b)
    {
        return (a && b) || ((!a) && (!b));
    }
    public bool IfFinish()
    {
        bool ans = true;
        if(!(FinishSituation[0]|| FinishSituation[1]|| FinishSituation[2]))
        {
            return false;
        }
        for (int i = 0; i < AngleKind.Length; i++)
        {
            ans = ans && TongHuo(FinishSituation[i], AngleKind[i]);
        }
        
        return ans;
    }

}