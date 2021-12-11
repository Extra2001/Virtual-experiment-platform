/************************************************************************************
    作者：荆煦添
    描述：右键物体信息数据模型
*************************************************************************************/
using DG.Tweening;
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
    public GameObject _RootPanel;
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
    [SerializeField]
    private Button _ResetStep;
    [SerializeField]
    private Text _StepCount;
    [SerializeField]
    private Button _Righting;

    private ObjectValue objectValue;
    private int axis = 0;

    private void Start()
    {
        _Step.text = "1";
        _Mask.onClick.AddListener(Hide);
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
        _MassInput.onValueChanged.AddListener(x => objectValue.Mass = float.Parse(x));
        _Gravity.onValueChanged.AddListener(x => objectValue.Gravity = x);
        _StepAdd.onClick.AddListener(() =>
        {
            // 要步进缩放的轴
            var axis = _StepAxis.selectedSegmentIndex;
            var step = Convert.ToDouble(_Step.text);
            Debug.Log(axis);
            Debug.Log(step);
            Debug.Log(objectValue.Scale);
            if (axis == 0)
            {
                objectValue.LineScaleZ = objectValue.Scale;
                objectValue.LineScaleY = objectValue.Scale;
                objectValue.LineScaleX = objectValue.LineScaleX + objectValue.Scale * (float)step;
                _StepCount.text = ((objectValue.LineScaleX - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
            if (axis == 1)
            {
                objectValue.LineScaleZ = objectValue.Scale;
                objectValue.LineScaleX = objectValue.Scale;
                objectValue.LineScaleY = objectValue.LineScaleY + objectValue.Scale * (float)step;
                _StepCount.text = ((objectValue.LineScaleY - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
            if (axis == 2)
            {
                objectValue.LineScaleX = objectValue.Scale;
                objectValue.LineScaleY = objectValue.Scale;
                objectValue.LineScaleZ = objectValue.LineScaleZ + objectValue.Scale * (float)step;
                _StepCount.text = ((objectValue.LineScaleZ - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
            Debug.Log("Lx=" + objectValue.LineScaleX);
            Debug.Log("Ly=" + objectValue.LineScaleY);
            Debug.Log("Lz=" + objectValue.LineScaleZ);
            // 步进增加
        });
        _StepSub.onClick.AddListener(() =>
        {
            // 要步进缩放的轴
            var axis = _StepAxis.selectedSegmentIndex;
            var step = Convert.ToDouble(_Step.text);

            // 步进减少
            if (axis == 0)
            {
                objectValue.LineScaleZ = objectValue.Scale;
                objectValue.LineScaleY = objectValue.Scale;
                objectValue.LineScaleX = Math.Max(1, objectValue.LineScaleX - objectValue.Scale * (float)step);
                _StepCount.text = ((objectValue.LineScaleX - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
            if (axis == 1)
            {
                objectValue.LineScaleZ = objectValue.Scale;
                objectValue.LineScaleX = objectValue.Scale;
                objectValue.LineScaleY = Math.Max(1, objectValue.LineScaleY - objectValue.Scale * (float)step);
                _StepCount.text = ((objectValue.LineScaleY - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
            if (axis == 2)
            {
                objectValue.LineScaleX = objectValue.Scale;
                objectValue.LineScaleY = objectValue.Scale;
                objectValue.LineScaleZ = Math.Max(1, objectValue.LineScaleZ - objectValue.Scale * (float)step);
                _StepCount.text = ((objectValue.LineScaleZ - objectValue.Scale) / ((float)step * objectValue.Scale)).ToString("F0");
            }
        });
        _ResetStep.onClick.AddListener(() => { objectValue.Scale = objectValue.Scale; _StepCount.text = "0"; });
        _Righting.onClick.AddListener(RightIt);
        _StepAxis.onValueChanged.AddListener(x => new int().Equals(x != -1 ? axis = x : x));
    }

    public void RightIt()
    {
        var TarX = objectValue.Rotation.eulerAngles.x;
        var TarY = objectValue.Rotation.eulerAngles.y;
        var TarZ = objectValue.Rotation.eulerAngles.z;
        if (TarX < 45 || TarX > 315)
            TarX = 0;
        else if (TarX < 135 && TarX > 45)
            TarX = 90;
        else if (TarX < 225 && TarX > 135)
            TarX = 180;
        else if (TarX < 315 && TarX > 225)
            TarX = 270;
        if (TarY < 45 || TarY > 315)
            TarY = 0;
        else if (TarY < 135 && TarY > 45)
            TarY = 90;
        else if (TarY < 225 && TarY > 135)
            TarY = 180;
        else if (TarY < 315 && TarY > 225)
            TarY = 270;
        if (TarZ < 45 || TarZ > 315)
            TarZ = 0;
        else if (TarZ < 135 && TarZ > 45)
            TarZ = 90;
        else if (TarZ < 225 && TarZ > 135)
            TarZ = 180;
        else if (TarZ < 315 && TarZ > 225)
            TarZ = 270;
        objectValue.gameObject.transform.DORotate(new Vector3(TarX, TarY, TarZ), 0.3f).SetEase(Ease.OutExpo);
        MainThread.Instance.DelayAndRun(300, () => objectValue.Rotation = new Vector3(TarX, TarY, TarZ).ToQuaternion());
        Hide();
    }

    public void Hide()
    {
        _RootPanel.transform.DOScale(0, 0.3f)
            .SetEase(Ease.OutExpo);
        Invoke(nameof(Close), 0.3f);
    }

    public void Close()
    {
        Main.m_UI.CloseUI<ObjectInfoUILogic>();
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
        _StepAxis.selectedSegmentIndex = axis;

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
    }
}
