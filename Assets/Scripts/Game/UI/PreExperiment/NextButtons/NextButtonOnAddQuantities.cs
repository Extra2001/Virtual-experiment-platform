using HT.Framework;
using UnityEngine.UI;

public class NextButtonOnAddQuantities : HTBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (ValueValidator.ValidateQuantities(RecordManager.tempRecord.quantities))
                GameManager.Instance.SwitchProcedure<EnterExpressionProcedure>();
        });
    }
}
