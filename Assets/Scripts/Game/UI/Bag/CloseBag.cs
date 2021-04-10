using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseBag : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(CloseIt);
    }

    private void CloseIt()
    {
        Main.m_UI.GetUI<BagControl>().Hide();
    }
}
