using HT.Framework;
using UnityEngine.UI;
using UnityEngine;

public class TipsHandler : HTBehaviour
{
    [Multiline]
    public string TipsContent;
    [Range(200, 500)]
    public float width = 200;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => UIAPI.Instance.ShowTips(TipsContent, width: width));
    }
}
