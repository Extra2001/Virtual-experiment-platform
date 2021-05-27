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
    public abstract double URV { get; }

    /// <summary>
    /// 下限
    /// </summary>
    public abstract double LRV { get; }

    /// <summary>
    /// 仪器误差限
    /// </summary>
    public abstract double ErrorLimit { get; }

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

    public virtual Sprite previewImage { get => prePreviewImage == null ? 
            (prePreviewImage = Resources.Load<Sprite>(previewImagePath)) : prePreviewImage; }

    /// <summary>
    /// 测量获取数据
    /// </summary>
    /// <returns>测量数据</returns>
    public abstract double GetMeasureResult();

    /// <summary>
    /// 重置仪器状态
    /// </summary>
    public abstract void InstReset();

    public abstract void ShowValue(double value);

    public override void OnShow()
    {
        Entity.layer = 11;
        Entity.tag = "Tools_Be_Moved";
        AddRightButton();

        var recpos = RecordManager.tempRecord.InstrumentStartPosition;
        Position.x = recpos[0];
        Position.y = recpos[1];
        Position.z = recpos[2];

        Entity.transform.position = Position;

        base.OnShow();
    }

    protected virtual void AddRightButton()
    {
        var right = Entity.AddComponent<RightButton>();
        right.InstrumentType = this.GetType();
    }

    public override void OnHide()
    {
        GameObject.Destroy(Entity.GetComponent<RightButton>());
    }

    public override void OnInit()
    {
        base.OnInit();
        Main.m_Resource.LoadAsset<Sprite>(new AssetInfo(null, null, previewImagePath), loadDoneAction: x =>
        {
            prePreviewImage = x;
        });
    }

    public virtual void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)
    {
        infoItems["_Name"].GameObject.GetComponent<Text>().text = InstName;
        infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
        infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
        infoItems["_Unit"].GameObject.GetComponent<Text>().text = Unit;
        infoItems["_UnitSymbol"].GameObject.GetComponent<Text>().text = UnitSymbol;
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
        });
    }
}
