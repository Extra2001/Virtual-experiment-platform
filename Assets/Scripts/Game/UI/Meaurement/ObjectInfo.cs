/************************************************************************************
    作者：荆煦添
    描述：右键物体信息数据模型
*************************************************************************************/
using HT.Framework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ObjectInfo : HTBehaviour
{
    [SerializeField]
    private Button _Mask;
    [SerializeField]
    private GameObject _RootPanel;
    [SerializeField]
    private Text _Title;
    [SerializeField]
    private Text _Desc;
    [SerializeField]
    private InputField _ScaleInput;
    [SerializeField]
    private Slider _ScaleSlider;
    [SerializeField]
    private InputField _MassInput;
    [SerializeField]
    private Toggle _Gravity;
    [SerializeField]
    private InputField _Step;
    [SerializeField]
    private SegmentedControl _StepAxis;
    [SerializeField]
    private Button _StepAdd;
    [SerializeField]
    private Button _StepSub;

    private ObjectValue objectValue;

    private void Start()
    {
        _Step.text = "1";
        _Mask.onClick.AddListener(() =>
        {
            Main.m_UI.CloseUI<ObjectInfoUILogic>();
        });
        _ScaleInput.onEndEdit.AddListener(x =>
        {
            var val = float.Parse(x);
            if (val > _ScaleSlider.maxValue)
                val = _ScaleSlider.maxValue;
            else if (val < _ScaleSlider.minValue)
                val = _ScaleSlider.minValue;
            objectValue.Scale = val;
            _ScaleSlider.value = val;
        });
        _ScaleSlider.onValueChanged.AddListener(x =>
        {
            objectValue.Scale = x;
            _ScaleInput.text = x.ToString();
        });
        _MassInput.onValueChanged.AddListener(x =>
        {
            objectValue.Mass = float.Parse(x);
        });
        _Gravity.onValueChanged.AddListener(x =>
        {
            objectValue.Gravity = x;
        });
        _StepAdd.onClick.AddListener(() =>
        {
            // 要步进缩放的轴
            var axis = _StepAxis.selectedSegmentIndex;
            var step = Convert.ToDouble(_Step.text);
            // 步进增加
        });
        _StepSub.onClick.AddListener(() =>
        {
            // 要步进缩放的轴
            var axis = _StepAxis.selectedSegmentIndex;
            var step = Convert.ToDouble(_Step.text);
            // 步进减少
        });
    }

    public void Show(ObjectValue value)
    {
        objectValue = value;
        _Title.text = objectValue.ObjectModel.Name;
        _Desc.text = objectValue.ObjectModel.DetailMessage;
        _ScaleInput.text = objectValue.Scale.ToString();
        _ScaleSlider.value = objectValue.Scale;
        _ScaleSlider.maxValue = objectValue.BaseScale * 5;
        _ScaleSlider.minValue = objectValue.BaseScale / 5;
        _MassInput.text = objectValue.Mass.ToString();
        _Gravity.isOn = objectValue.Gravity;

        if(value.ObjectModel.ResourcePath.EndsWith("obj") || Path.GetFileName(value.ObjectModel.ResourcePath).Contains("."))
        {
            _ScaleSlider.interactable = true;
            _ScaleInput.interactable = true;
        }
        else
        {
            _ScaleSlider.interactable = false;
            _ScaleInput.interactable = false;
        }

        StartCoroutine(CommonTools.DelayGet(_RootPanel.rectTransform().SetFloat));
    }
}
