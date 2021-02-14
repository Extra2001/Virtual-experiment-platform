﻿using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
#endif

namespace HT.Framework
{
    /// <summary>
    /// 步骤内容
    /// </summary>
    [Serializable]
    public sealed class StepContent
    {
        #region Base Property
        /// <summary>
        /// 步骤ID
        /// </summary>
        public string GUID = "";
        /// <summary>
        /// Enter节点的锚点
        /// </summary>
        [SerializeField] internal Vector2 EnterAnchor = Vector2.zero;
        /// <summary>
        /// 步骤执行到进入下一步的时间
        /// </summary>
        public float ElapseTime = 1;
        /// <summary>
        /// 步骤Enter节点开始执行的所有操作的总时间
        /// </summary>
        [SerializeField] internal float Totaltime = 0;
        /// <summary>
        /// 立即执行模式
        /// </summary>
        public bool Instant = false;
        /// <summary>
        /// 步骤目标
        /// </summary>
        public GameObject Target = null;
        [SerializeField] internal string TargetGUID = "<None>";
        [SerializeField] internal string TargetPath = "<None>";
        /// <summary>
        /// 步骤简述名称
        /// </summary>
        public string Name = "Step Name";
        /// <summary>
        /// 步骤触发方式
        /// </summary>
        public StepTrigger Trigger = StepTrigger.StateChange;
        /// <summary>
        /// 步骤详述信息
        /// </summary>
        public string Prompt = "Step Prompt";
        /// <summary>
        /// 步骤其他信息【目前普遍作为步骤大纲】
        /// </summary>
        public string Ancillary = "";
        /// <summary>
        /// 步骤最佳视角【自由控制】
        /// </summary>
        public Vector3 BestView = new Vector3(90, 30, 2);
        /// <summary>
        /// 步骤视点偏移【自由控制】
        /// </summary>
        public Vector3 ViewOffset = Vector3.zero;
        /// <summary>
        /// 步骤最佳位置【第一、第三人称】
        /// </summary>
        public Vector3 BestPos = Vector3.zero;
        /// <summary>
        /// 步骤初始控制模式
        /// </summary>
        public ControlMode InitialMode = ControlMode.FreeControl;
        /// <summary>
        /// 步骤助手类名
        /// </summary>
        [SerializeField] internal string Helper = "<None>";
        /// <summary>
        /// 步骤参数列表
        /// </summary>
        [SerializeField] internal List<StepParameter> Parameters = new List<StepParameter>();
        /// <summary>
        /// 步骤操作列表
        /// </summary>
        [SerializeField] internal List<StepOperation> Operations = new List<StepOperation>();
        /// <summary>
        /// 步骤连线列表
        /// </summary>
        [SerializeField] internal List<StepWired> Wireds = new List<StepWired>();
        #endregion

        #region Execute Method
        /// <summary>
        /// 执行步骤内容
        /// </summary>
        internal void Execute()
        {
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (Wireds[i].Left == -1)
                {
                    Execute(i);
                }
            }
        }
        private void Execute(int index)
        {
            int stepIndex = Wireds[index].Right;
            Operations[stepIndex].Execute();

            if (Operations[stepIndex].Instant)
            {
                for (int i = 0; i < Wireds.Count; i++)
                {
                    if (Wireds[i].Left == stepIndex)
                    {
                        Execute(i);
                    }
                }
            }
            else
            {
                Main.m_StepMaster.DelayExecute(() =>
                {
                    for (int i = 0; i < Wireds.Count; i++)
                    {
                        if (Wireds[i].Left == stepIndex)
                        {
                            Execute(i);
                        }
                    }
                }, Operations[stepIndex].ElapseTime);
            }
        }

        /// <summary>
        /// 跳过步骤内容
        /// </summary>
        internal void Skip()
        {
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (Wireds[i].Left == -1)
                {
                    Skip(i);
                }
            }
        }
        private void Skip(int index)
        {
            int stepIndex = Wireds[index].Right;
            Operations[stepIndex].Skip();

            if (Operations[stepIndex].Instant)
            {
                for (int i = 0; i < Wireds.Count; i++)
                {
                    if (Wireds[i].Left == stepIndex)
                    {
                        Skip(i);
                    }
                }
            }
            else
            {
                Main.m_StepMaster.DelayExecute(() =>
                {
                    for (int i = 0; i < Wireds.Count; i++)
                    {
                        if (Wireds[i].Left == stepIndex)
                        {
                            Skip(i);
                        }
                    }
                }, Operations[stepIndex].ElapseTime / StepMaster.SkipMultiple);
            }
        }
        #endregion

        #region EditorOnly
#if UNITY_EDITOR
        internal static float Width = 150;
        internal static float Height = 40;

        /// <summary>
        /// 步骤助手名称
        /// </summary>
        internal string HelperName { get; private set; } = null;
        /// <summary>
        /// Enter节点的位置
        /// </summary>
        internal Rect EnterPosition
        {
            get
            {
                return new Rect(EnterAnchor.x - Width / 2, EnterAnchor.y - Height / 2, Width, Height);
            }
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns>新的对象</returns>
        internal StepContent Clone()
        {
            StepContent content = new StepContent();
            content.GUID = Guid.NewGuid().ToString();
            content.EnterAnchor = EnterAnchor;
            content.ElapseTime = ElapseTime;
            content.Instant = Instant;
            content.Target = Target;
            content.TargetGUID = TargetGUID;
            content.TargetPath = TargetPath;
            content.Name = Name;
            content.Trigger = Trigger;
            content.Prompt = Prompt;
            content.Ancillary = Ancillary;
            content.BestView = BestView;
            content.ViewOffset = ViewOffset;
            content.BestPos = BestPos;
            content.InitialMode = InitialMode;
            content.Helper = Helper;
            for (int i = 0; i < Parameters.Count; i++)
            {
                content.Parameters.Add(Parameters[i].Clone());
            }
            for (int i = 0; i < Operations.Count; i++)
            {
                content.Operations.Add(Operations[i].Clone());
            }
            for (int i = 0; i < Wireds.Count; i++)
            {
                content.Wireds.Add(Wireds[i].Clone());
            }
            return content;
        }
        /// <summary>
        /// 是否存在指定连线
        /// </summary>
        /// <param name="left">连线左侧</param>
        /// <param name="right">连线右侧</param>
        /// <returns>是否存在</returns>
        internal bool IsExistWired(int left, int right)
        {
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (Wireds[i].Left == left && Wireds[i].Right == right)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 当前步骤包含指定类型的操作数量
        /// </summary>
        /// <param name="type">操作类型</param>
        /// <returns>包含数量</returns>
        internal int GetOperationsCout(StepOperationType type)
        {
            int cout = 0;
            for (int i = 0; i < Operations.Count; i++)
            {
                if (Operations[i].OperationType == type)
                {
                    cout += 1;
                }
            }
            return cout;
        }
        /// <summary>
        /// 获取可能会被执行两次或以上的节点
        /// </summary>
        /// <param name="indexs">节点索引集合</param>
        internal void GetExecuteTwice(HashSet<int> indexs)
        {
            HashSet<int> sign = new HashSet<int>();
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (sign.Contains(Wireds[i].Right))
                {
                    indexs.Add(Wireds[i].Right);
                }
                else
                {
                    sign.Add(Wireds[i].Right);
                }
            }
        }
        /// <summary>
        /// 获取所有执行终点
        /// </summary>
        /// <param name="indexs">节点索引集合</param>
        internal void GetTerminus(HashSet<int> indexs)
        {
            HashSet<int> sign = new HashSet<int>();
            for (int i = 0; i < Wireds.Count; i++)
            {
                sign.Add(Wireds[i].Left);
            }

            for (int i = 0; i < Operations.Count; i++)
            {
                if (!sign.Contains(i))
                {
                    indexs.Add(i);
                }
            }
        }
        /// <summary>
        /// 计算节点到Enter节点的执行总时间
        /// </summary>
        /// <param name="index">节点索引</param>
        /// <returns>总时间</returns>
        internal float ComputeTotalTime(int index)
        {
            float totalTime = 0;

            int left = index;
            while (left >= 0)
            {
                totalTime += Operations[left].Instant ? 0 : Operations[left].ElapseTime;
                left = GetLeftIndex(left);
            }

            if (left == -1)
            {
                return totalTime;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取连线的左侧节点
        /// </summary>
        /// <param name="rightIndex">右侧节点</param>
        /// <returns>左侧节点</returns>
        internal int GetLeftIndex(int rightIndex)
        {
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (Wireds[i].Right == rightIndex)
                {
                    return Wireds[i].Left;
                }
            }
            return -2;
        }
        /// <summary>
        /// 获取节点的左侧相连节点
        /// </summary>
        /// <param name="rightIndex">右侧节点</param>
        /// <returns>左侧节点</returns>
        internal StepOperation GetLeftOperation(int rightIndex)
        {
            for (int i = 0; i < Wireds.Count; i++)
            {
                if (Wireds[i].Right == rightIndex)
                {
                    if (Wireds[i].Left == -1)
                    {
                        return null;
                    }
                    else
                    {
                        return Operations[Wireds[i].Left];
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 焦点到步骤目标
        /// </summary>
        internal void FocusTarget()
        {
            if (Target)
            {
                if (Selection.activeGameObject != Target)
                {
                    Selection.activeGameObject = Target;
                    EditorGUIUtility.PingObject(Target);
                }
            }
        }
        /// <summary>
        /// 更新步骤助手的名称
        /// </summary>
        internal void RefreshHelperName()
        {
            if (Helper == "<None>")
            {
                HelperName = "未设置助手";
            }
            else
            {
                Type type = ReflectionToolkit.GetTypeInRunTimeAssemblies(Helper);
                if (type == null)
                {
                    HelperName = "无效的助手";
                }
                else
                {
                    CustomHelperAttribute helper = type.GetCustomAttribute<CustomHelperAttribute>();
                    if (helper != null)
                    {
                        HelperName = helper.Name;
                    }
                    else
                    {
                        HelperName = type.FullName;
                    }
                }
            }
        }
#endif
        #endregion
    }
}