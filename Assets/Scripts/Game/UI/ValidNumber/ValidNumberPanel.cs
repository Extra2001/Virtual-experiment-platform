using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ValueNumberVariableModel
{
    public string Symbol;
    public string Value;
    public string Uncertainty;
}

public class ValidNumberPanel : HTBehaviour
{
    [SerializeField]
    private Button _AddNewPanel;
    [SerializeField]
    private GameObject _VarPanel;
    [SerializeField]
    private GameObject _VarPanelRoot;

    [SerializeField]
    private InputField _Expression;
    [SerializeField]
    private Image _Image;

    private Dictionary<GameObject, ValueNumberVariableModel> addedVarPanels 
        = new Dictionary<GameObject, ValueNumberVariableModel>();

    public void Initialize()
    {
        foreach (var item in addedVarPanels)
            Destroy(item.Key);
        addedVarPanels.Clear();
    }

    public void AddVariable()
    {
        var obj = Instantiate(_VarPanel, _VarPanelRoot.transform);
        var panel = obj.GetComponent<VariablePanel>();
        var model = new ValueNumberVariableModel()
        {
            Value = "0",
            Uncertainty = "0"
        };
        panel.Model = model;
        panel.Panel = this;

        addedVarPanels.Add(obj, model);
        _AddNewPanel.transform.SetAsLastSibling();
    }

    public void DeleteVariable(GameObject obj)
    {
        addedVarPanels.Remove(obj);
        Destroy(obj);
    }

    public void AddExpr(string add)
    {
        _Expression.text += add;
    }

    public void Render()
    {
        var text = _Expression.text;
        LatexEquationRender.Render(CalcArgs.GetSymexpr(text).ToLaTeX(), x =>
        {
            _Image.FitHeight(x);
        });
    }

    void Start()
    {
        _AddNewPanel.onClick.AddListener(AddVariable);
        _Expression.onValueChanged.AddListener(x =>
        {
            Render();
        });
        Initialize();
    }
}
