using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearAllData : HTBehaviour
{
    [SerializeField]
    private Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            UIAPI.Instance.ShowModel(new ModelDialogModel()
            {
                Message = new BindableString("将会清空所有数据，继续？"),
                ConfirmAction = () =>
                {
                    GameManager.Instance.ClearAll();
                }
            });
        });
    }
}
