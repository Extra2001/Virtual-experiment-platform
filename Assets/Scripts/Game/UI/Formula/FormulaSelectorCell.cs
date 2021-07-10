/************************************************************************************
    ���ߣ�������
    ��������ʽ�༭��ѡ�������������
*************************************************************************************/
using HT.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FormulaSelectorCell : HTBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ValueType
    {
        Normal,
        Measured,
        Complex
    }

    public FormulaController FormulaControllerInstance;
    [SerializeField]
    public string Title;
    [SerializeField]
    public string Desc;
    [Space]
    [SerializeField]
    private Text Text;
    [Space]
    [SerializeField]
    private ValueType valueType;
    [SerializeField]
    private MeasuredStatisticValue measuredStatisticValue;
    [SerializeField]
    private ComplexStatisticValue ComplexStatisticValue;
    [Space]
    public string value;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            string name = "x";
            if (valueType == ValueType.Measured)
            {
                value = (Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure)?.GetStatisticValue(measuredStatisticValue);
                name = (Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure)?.GetStatisticValue(MeasuredStatisticValue.Symbol);
            }
            else if (valueType == ValueType.Complex)
            {
                value = (Main.m_Procedure.CurrentProcedure as ComplexDataProcessProcedure)?.GetStatisticValue(Text.text, ComplexStatisticValue);
                name = Text.text;
            }
            FormulaControllerInstance.SelectCell(gameObject.name, value, name);
        });
    }
    /// <summary>
    /// ����ѡ����������
    /// </summary>
    /// <param name="name"></param>
    public void SetSelectorName(string name)
    {
        if (Text)
            Text.text = name;
    }
    /// <summary>
    /// ����뿪
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke();
        FormulaControllerInstance.Indicator.Hide();
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Invoke("ShowIndicator", 0.8f);
    }
    /// <summary>
    /// ��ʾ�������
    /// </summary>
    private void ShowIndicator()
    {
        if (valueType == ValueType.Measured)
            value = (Main.m_Procedure.CurrentProcedure as MeasuredDataProcessProcedure)?.GetStatisticValue(measuredStatisticValue);
        else if (valueType == ValueType.Complex)
            value = (Main.m_Procedure.CurrentProcedure as ComplexDataProcessProcedure)?.GetStatisticValue(Text.text, ComplexStatisticValue);
        FormulaControllerInstance.Indicator.ShowIndicate(Title, Desc, value);
        FormulaControllerInstance.RefreshContentSizeFitter(FormulaControllerInstance.Indicator.gameObject);
    }
}