/************************************************************************************
    ×÷Õß£º¾£ìãÌí
    ÃèÊö£º³éÌë¶¯»­
*************************************************************************************/
using UnityEngine;

public class InstrumentChooserAnimate : MonoBehaviour
{
    public void Show()
    {
        UIShowHideHelper.ShowFromButtom(gameObject, 0);
    }

    public void Hide()
    {
        UIShowHideHelper.HideToButtom(gameObject);
    }
}
