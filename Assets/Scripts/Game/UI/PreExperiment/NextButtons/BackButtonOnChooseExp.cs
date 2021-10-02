using HT.Framework;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonOnChooseExp : HTBehaviour
{
    //启用自动化
    protected override bool IsAutomate => true;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(
            GameManager.Instance.SwitchBackToStart);
    }
}
