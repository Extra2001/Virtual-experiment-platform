/************************************************************************************
    作者：张柯、张峻凡、荆煦添
    描述：所有仪器的公共属性和行为
*************************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using HT.Framework;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Text.RegularExpressions;

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
    bool ShowValue(double value);
}

public interface IShowInfoPanelable
{
    void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems);
}

public abstract class InstrumentBase : EntityLogicBase, IMeasurable, IResetable, IShowValuable, IShowInfoPanelable
{
    public Vector3 Position = new Vector3();
    /// <summary>
    /// 排序ID
    /// </summary>
    public virtual int ID { get; } = int.MaxValue;
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
    /// <summary>
    /// 缩略图
    /// </summary>
    public abstract string previewImagePath { get; }
    private Sprite prePreviewImage = null;
    public virtual Sprite previewImage
    {
        get => prePreviewImage == null ?
            (prePreviewImage = Resources.Load<Sprite>(previewImagePath)) : prePreviewImage;
    }
    private Vector3 initPosition = new Vector3();
    private Quaternion initRotation = new Quaternion();

    /// <summary>
    /// 测量获取数据
    /// </summary>
    /// <returns>测量数据</returns>
    public abstract double GetMeasureResult();

    public virtual bool ShowValue(double value)
    {
        if (value > URV || value < LRV)
        {
            UIAPI.Instance.ShowModel(new SimpleModel()
            {
                ShowCancel = false,
                Message = "主值不能超过量程"
            });
            return false;
        }
        MainValue = value;
        return true;
    }

    public virtual void GenMainValueAndRandomErrorLimit()//随机生成主值和误差
    {
        MainValue = (new System.Random().NextDouble() * (URV - LRV)) + LRV;
    }

    /// <summary>
    /// 重置仪器状态
    /// </summary>
    public virtual void InstReset()
    {
        Entity.transform.GetChild(0).localPosition = initPosition;
        Entity.transform.GetChild(0).localRotation = initRotation;
        RandomErrorLimit = 0;
        ShowValue(0);
    }

    protected virtual void AddRightButton()//添加右键菜单功能
    {
        RightButton right;
        if ((right = Entity.GetComponent<RightButton>()) == null)
            right = Entity.AddComponent<RightButton>();
        right.InstrumentType = this.GetType();
    }

    public virtual void ShowInfoPanel(Dictionary<string, IntrumentInfoItem> infoItems)//右键菜单展示哪些信息
    {
        var keys = new string[] { "_Name", "_LRV", "_URV", "_Unit", "_UnitSymbol", "_Mask", "_RootPanel", "_Righting" };
        infoItems["_Name"].GameObject.GetComponent<Text>().text = InstName;
        infoItems["_LRV"].GameObject.GetComponent<Text>().text = LRV.ToString();
        infoItems["_URV"].GameObject.GetComponent<Text>().text = URV.ToString();
        infoItems["_Unit"].GameObject.GetComponent<Text>().text = Unit;
        infoItems["_UnitSymbol"].GameObject.GetComponent<Text>().text = UnitSymbol;

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

    public virtual (bool, string) CheckErrorLimit(string value)
    {
        if (Convert.ToDouble(value) == 0.0) return (true, null);

        var str = ErrorLimit.ToString();
        if (str.Contains("."))
            str = str.TrimEnd('0');
        if (str.EndsWith("."))
            str = str.Remove(str.Length - 1, 1);

        if (str.Contains("."))
        {
            var rightModel = Regex.Replace(str, @"[0-9]", "0", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var right = Convert.ToDecimal(value).ToString(rightModel);
            if (value.IndexOf('.') != value.LastIndexOf('.'))
                return (false, right);
            if (value.Contains("."))
            {
                var s1 = str.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                var s2 = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Last();
                if (s1.Length != s2.Length) return (false, right);
                return (true, null);
            }
            else return (false, right);
        }
        else
        {
            var match = Regex.Match(str, "[1-9]", RegexOptions.IgnoreCase | RegexOptions.RightToLeft | RegexOptions.Compiled);
            var zeroCount = str.Length - match.Index - 1;
            if (zeroCount == 0)
            {
                if (value.Contains("."))
                    return (false, value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First());
                else return (true, null);
            }
            var input = value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First();
            if (input.Length < zeroCount)
                return (false, "0");

            string a = "";
            for (int i = 0; i < zeroCount; i++) a += "0";

            var right = Regex.Replace(input, @"\d{" + zeroCount + "}$", a);
            if (value.Contains("."))
                return (false, right);
            var userZeroCount = Regex.Match(value, "0+$").ToString().Length;
            if (zeroCount != userZeroCount)
                return (false, right);
            return (true, null);
        }
    }

    public virtual (bool, string) CheckULLimit(string value)
    {
        var val = Convert.ToDouble(value);
        if (val > URV || val < LRV)
            return (false, "记录值超出仪器的量程范围");
        return (true, null);
    }

    public override void OnShow()//仪器展示的时候调用一次
    {
        Entity.transform.GetChild(0).gameObject.SetActive(true);
        base.OnShow();
        AddRightButton();
    }

    public override void OnUpdate()//每帧调用一次
    {
        base.OnUpdate();
        if (Entity.activeSelf && Entity.transform.GetChild(0).gameObject.activeSelf)
        {
            RecordManager.tempRecord.showedInstrument.Valid = true;
            RecordManager.tempRecord.showedInstrument.MainValue = MainValue;
            RecordManager.tempRecord.showedInstrument.RandomErrorLimit = RandomErrorLimit;
            RecordManager.tempRecord.showedInstrument.position = Entity.transform.GetChild(0).position.GetMyVector();
            RecordManager.tempRecord.showedInstrument.rotation = Entity.transform.GetChild(0).rotation.GetMyVector();
        }
    }

    public override void OnHide()//仪器隐藏时调用
    {
        Entity.transform.GetChild(0).gameObject.SetActive(false);
        if (!RecordManager.tempRecord.historyInstrument.Contains(RecordManager.tempRecord.showedInstrument))
            RecordManager.tempRecord.historyInstrument.Add(RecordManager.tempRecord.showedInstrument);
        InstReset();
    }

    public override void OnInit()//初始化时调用
    {
        base.OnInit();
        initPosition = Entity.transform.GetChild(0).localPosition;
        initRotation = Entity.transform.GetChild(0).localRotation;
        Main.m_Resource.LoadAsset<Sprite>(new AssetInfo(null, null, previewImagePath), loadDoneAction: x =>
        {
            prePreviewImage = x;
        });
    }

    public override void Reset()//重置时调用
    {
        base.Reset();
        InstReset();
    }
}
