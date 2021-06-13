using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IMeasurable
{
    double GetMeasureResult();
}

public interface IResetable
{
    void InstReset();
}

public interface IShowValuable
{
    void ShowValue(double value);
}

public interface IShowInfoPanelable
{
    void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems);
}

public abstract class InstrumentBase : EntityLogicBase, IMeasurable, IResetable, IShowValuable, IShowInfoPanelable
{
    public Vector3 Position = new Vector3();
    /// <summary>
    /// 仪器名称
    /// </summary>
    public abstract string InstName { get; }

    /// <summary>
    /// 上限
    /// </summary>
    public abstract double URV { get; set; }

    /// <summary>
    /// 下限
    /// </summary>
    public abstract double LRV { get; set; }

    /// <summary>
    /// 仪器误差限
    /// </summary>
    public abstract double ErrorLimit { get; set; }

    /// <summary>
    /// 随机误差
    /// </summary>
    public virtual double RandomError { get => UnityEngine.Random.Range(-1 * (float)RandomErrorLimit, (float)RandomErrorLimit); }

    /// <summary>
    /// 随机误差限
    /// </summary>
    public abstract double RandomErrorLimit { get; set; }

    /// <summary>
    /// 主值
    /// </summary>
    public abstract double MainValue { get; set; }

    /// <summary>
    /// 单位名称
    /// </summary>
    public abstract string Unit { get; }

    /// <summary>
    /// 单位符号
    /// </summary>
    public abstract string UnitSymbol { get; }

    public abstract string previewImagePath { get; }

    private Sprite prePreviewImage = null;

    public virtual Sprite previewImage
    {
        get => prePreviewImage == null ?
            (prePreviewImage = Resources.Load<Sprite>(previewImagePath)) : prePreviewImage;
    }

    /// <summary>
    /// 测量获取数据
    /// </summary>
    /// <returns>测量数据</returns>
    public abstract double GetMeasureResult();

    public abstract void ShowValue(double value);

    public virtual void GenMainValueAndRandomErrorLimit()
    {
        MainValue = (new System.Random().NextDouble() * (URV - LRV)) + LRV;
    }

    /// <summary>
    /// 重置仪器状态
    /// </summary>
    public virtual void InstReset()
    {
        MainValue = 0;
        RandomErrorLimit = 0;
        ShowValue(0);
    }

    protected virtual void AddRightButton()
    {
        RightButton right;
        if ((right = Entity.GetComponent<RightButton>()) == null)
            right = Entity.AddComponent<RightButton>();
        right.InstrumentType = this.GetType();
    }

    public virtual void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)
    {
        var keys = new string[] { "_Name", "_LRV", "_URV", "_Unit", "_UnitSymbol", "_Mask", "_RootPanel" };
        infoItems["_Name"].GameObject.GetComponent<Text>().text = InstName;
        infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
        infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
        infoItems["_Unit"].GameObject.GetComponent<Text>().text = Unit;
        infoItems["_UnitSymbol"].GameObject.GetComponent<Text>().text = UnitSymbol;
        /*
        infoItems["_MainValue"].GameObject.GetComponent<InputField>().text = MainValue.ToString();
        infoItems["_RandomError"].GameObject.GetComponent<InputField>().text = RandomErrorLimit.ToString();
        infoItems["_ConfirmButton"].onValueChanged.Add(() =>
        {
            double re = Convert.ToDouble(infoItems["_RandomError"].GameObject.GetComponent<InputField>().text);
            double mainValue = Convert.ToDouble(infoItems["_MainValue"].GameObject.GetComponent<InputField>().text);
            if (re > ErrorLimit)
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("随机误差不能大于仪器误差限")
                });
            else if (mainValue > URV || mainValue < LRV)
            {
                UIAPI.Instance.ShowModel(new ModelDialogModel()
                {
                    ShowCancel = false,
                    Message = new BindableString("主值不能超过量程")
                });
            }
            else
            {
                RandomErrorLimit = re;
                MainValue = mainValue;
                ShowValue(mainValue);
            }
        });*/  //已迁移至IndirectInstrumentBase
        foreach (var item in infoItems)
        {
            if (!keys.Contains(item.Key))
            {
                item.Value.GameObject.SetActive(false);
            }
        }
    }

    public virtual void ShowGameButton(List<GameButtonItem> buttonItems)
    {
        
    }

    public override void OnShow()
    {
        Entity.transform.GetChild(0).gameObject.SetActive(true);
        base.OnShow();
        AddRightButton();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Entity.activeSelf)
        {
            RecordManager.tempRecord.showedInstrument.Valid = true;
            RecordManager.tempRecord.showedInstrument.MainValue = MainValue;
            RecordManager.tempRecord.showedInstrument.RandomErrorLimit = RandomErrorLimit;
            RecordManager.tempRecord.showedInstrument.position = Entity.transform.GetChild(0).position.GetMyVector();
            RecordManager.tempRecord.showedInstrument.rotation = Entity.transform.GetChild(0).rotation.GetMyVector();
        }
    }

    public override void OnHide()
    {
        Entity.transform.GetChild(0).gameObject.SetActive(false);
    }

    public override void OnInit()
    {
        base.OnInit();
        Main.m_Resource.LoadAsset<Sprite>(new AssetInfo(null, null, previewImagePath), loadDoneAction: x =>
        {
            prePreviewImage = x;
        });
    }

    public override void Reset()
    {
        base.Reset();
        InstReset();
    }
}
