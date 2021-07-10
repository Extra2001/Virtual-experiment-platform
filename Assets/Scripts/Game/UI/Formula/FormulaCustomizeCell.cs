/************************************************************************************
    作者：荆煦添
    描述：公式编辑器选择器自定义方块绑定数据
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class FormulaCustomizeCell : HTBehaviour
{
    public Text show;
    public FormulaSelectorCell FormulaSelectorCell;
    public InputField input;
    
    void Start()
    {
        input.onValueChanged.AddListener(value =>
        {
            if (string.IsNullOrEmpty(value))
            {
                FormulaSelectorCell.value = $"0";
                show.text = "0";
                return;
            }
            FormulaSelectorCell.value = $"{value}";
            show.text = value;
        });
    }
}
