using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class KeyboardManager:SingletonBehaviorManager<KeyboardManager>
{
    //此脚本控制非连续按下的所有键盘按键（移动及卡尺夹紧除外），下行标注所有按键作用
    //ESC：暂停，E：坐上凳子，B：打开关闭背包，~~~~~~~~~~~~~~

    private ProcedureBase CurrentProcedure;
    private bool InputAble = true;

    // Update is called once per frame
    void Update()
    {
        CurrentProcedure=Main.m_Procedure.CurrentProcedure;

        EscListener();
        BListener();


    }

    private void InputGap()
    {
        InputAble = false;
        //禁止300ms内连续按下两个按键
        Task.Delay(300).ContinueWith((_) =>
        {
            InputAble = true;
        });
    }

    private void EscListener()
    {
        if (CurrentProcedure is StartProcedure || CurrentProcedure is EnterClassroomProcedure || CurrentProcedure is OnChair)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //jxt后续补上
            }
        }       
    }

    private void BListener()
    {
        if (CurrentProcedure is OnChair)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Main.m_UI.OpenTemporaryUI<BagControl>();
                Debug.Log("B");
            }
        }
    }
}
