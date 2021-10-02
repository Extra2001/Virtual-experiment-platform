using HT.Framework;
using UnityEngine.UI;

public class NextButtonOnPreviewConfirm : HTBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            UIAPI.Instance.ShowLoading();
            MainThread.Instance.DelayAndRun(500,
                GameManager.Instance.SwitchProcedure<OnChairProcedure>);
        });
    }
}
