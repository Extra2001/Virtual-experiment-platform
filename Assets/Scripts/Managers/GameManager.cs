/************************************************************************************
    作者：张峻凡、荆煦添
    描述：游戏核心管理器，管理游戏的所有资源和配置缓存
*************************************************************************************/
using HT.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common;
public class GameManager : SingletonBehaviorManager<GameManager>
{
    #region 属性访问器
    List<Type> ProcedureStack { get => RecordManager.tempRecord.procedureStack; }
    public bool CanContinue { get => ProcedureStack.Count > 0; }
    public int _currentQuantityIndex
    {
        get => RecordManager.tempRecord.currentQuantityIndex;
        set => RecordManager.tempRecord.currentQuantityIndex = value;
    }
    public QuantityModel CurrentQuantity
    {
        get => RecordManager.tempRecord.quantities[_currentQuantityIndex];
    }
    private List<ObjectsModel> _objectsModels = null;
    public List<ObjectsModel> objectsModels
    {
        get => _objectsModels == null ?
            _objectsModels = Storage.UnityStorage.GetStorage<List<ObjectsModel>>("objectsModels", null, null) :
            _objectsModels;
        set => _objectsModels = value;
    }
    public InstrumentBase CurrentInstrument => GetInstrument(RecordManager.tempRecord.showedInstrument);
    public bool FPSable
    {
        get => firstPersonController.gameObject.activeSelf;
        set
        {
            firstPersonController.gameObject.GetComponent<CharacterController>().enabled = value;
            firstPersonController.enabled = value;
        }
    }
    public bool Movable
    {
        get => firstPersonController.m_WalkSpeed > 0.1;
        set
        {
            if (value)
            {
                firstPersonController.m_WalkSpeed = 30;
                firstPersonController.m_RunSpeed = 50;
            }
            else
                firstPersonController.m_RunSpeed = firstPersonController.m_WalkSpeed = 0;
        }
    }
    public Vector3 PersonPosition
    {
        get => firstPersonController.transform.position;
        set => firstPersonController.transform.position = value;
    }
    public Quaternion PersonRotation
    {
        get => firstPersonController.transform.rotation;
        set => firstPersonController.transform.rotation = value;
    }
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController firstPersonController = null;
    #endregion

    #region Unity生命周期函数
    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        firstPersonController = GameObject.Find("FPSController")
            .GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        FPSable = false;
        Main.m_Event.Subscribe<BeforeClearTempRecordEventHandler>(ClearObjects);
        InvokeRepeating(nameof(SaveRecord), 10, 30);
    }

    public void ClearAll()
    {
        Storage.DeleteAll();
        RecordManager.ClearTempRecord();
        Main.m_Procedure.SwitchProcedure<StartProcedure>();
        ProcedureStack.Clear();
    }

    /// <summary>
    /// 存档读取保存与清理
    /// </summary>
    private void SaveRecord()
    {
        RecordManager.tempRecord.Save();
        Storage.UnityStorage.SetStorage("objectsModels", _objectsModels);
    }
    #endregion

    public void ClearObjects()
    {
        CreateObject.HideCurrent();
        CreateInstrument.HideCurrent();
    }

    public InstrumentBase GetInstrument(InstrumentInfoModel model)
    {
        if (model == null) return null;
        return GetInstrument(model.instrumentType);
    }
    public InstrumentBase GetInstrument(Type type)
    {
        return Main.m_Entity.GetEntity(type, type.Name) as InstrumentBase;
    }

    #region 流程控制
    public void SwitchBackToStart()
    {
        Main.m_Procedure.SwitchProcedure<StartProcedure>();
    }

    public void SwitchBackProcedure()
    {
        if (ProcedureStack.Count <= 1)
        {
            SwitchBackToStart();
            return;
        }
        ProcedureStack.RemoveAt(ProcedureStack.Count - 1);
        Main.m_Procedure.SwitchProcedure(ProcedureStack[ProcedureStack.Count - 1]);
    }

    public void SwitchProcedure<T>() where T : ProcedureBase
    {
        Main.m_Procedure.SwitchProcedure<T>();
        if (ProcedureStack.Count == 0)
            ProcedureStack.Add(typeof(T));
        else if(!ProcedureStack.Last().Name.Equals(typeof(T).Name))
            ProcedureStack.Add(typeof(T));
    }

    public void ContinueExp()
    {
        if (ProcedureStack.Count > 0)
            Main.m_Procedure.SwitchProcedure(ProcedureStack.Last());
    }

    public void ChooseNewExp()
    {
        Main.m_Procedure.SwitchProcedure<ChooseExpProcedure>();
    }

    public void StartValidNumber()
    {
        Main.m_Procedure.SwitchProcedure<UncertainLearnProcedure>();
    }

    public void ShowUncertainty()
    {
        var pro = Main.m_Procedure.GetProcedure<MeasuredDataProcessProcedure>();
        pro.ShowUncertainty(CurrentQuantity);
    }
    #endregion
}
