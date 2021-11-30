using System.Collections.Generic;
using System.Reflection;

namespace HT.Framework
{
    /// <summary>
    /// 默认的切面代理追踪器助手
    /// </summary>
    public sealed class DefaultAspectTrackHelper
    {
        /// <summary>
        /// 切面代理追踪器
        /// </summary>
        public InternalModuleBase Module { get; set; }
        /// <summary>
        /// 所有的代理对象【真实对象、代理对象】
        /// </summary>
        public Dictionary<IAspectTrackObject, IAspectTrackObject> ProxyObjects { get; private set; } = new Dictionary<IAspectTrackObject, IAspectTrackObject>();
        /// <summary>
        /// 所有的代理者【真实对象，代理者】
        /// </summary>
        public Dictionary<IAspectTrackObject, object> Proxys { get; private set; } = new Dictionary<IAspectTrackObject, object>();
        /// <summary>
        /// 全局拦截条件
        /// </summary>
        public Dictionary<string, HTFFunc<MethodBase, object[], bool>> InterceptConditions { get; private set; } = new Dictionary<string, HTFFunc<MethodBase, object[], bool>>();

        /// <summary>
        /// 初始化助手
        /// </summary>
        public void OnInitialization()
        { }
        /// <summary>
        /// 助手准备工作
        /// </summary>
        public void OnPreparatory()
        { }
        /// <summary>
        /// 刷新助手
        /// </summary>
        public void OnRefresh()
        { }
        /// <summary>
        /// 终结助手
        /// </summary>
        public void OnTermination()
        {
            ClearInterceptCondition();
        }
        /// <summary>
        /// 暂停助手
        /// </summary>
        public void OnPause()
        { }
        /// <summary>
        /// 恢复助手
        /// </summary>
        public void OnUnPause()
        { }

        /// <summary>
        /// 新增拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <param name="interceptCondition">拦截规则</param>
        public void AddInterceptCondition(string conditionName, HTFFunc<MethodBase, object[], bool> interceptCondition)
        {
            if (!InterceptConditions.ContainsKey(conditionName))
            {
                InterceptConditions.Add(conditionName, interceptCondition);
            }
            else
            {
                Log.Warning("新增拦截条件失败：已存在同名拦截条件 " + conditionName + " ！");
            }
        }
        /// <summary>
        /// 是否存在拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <returns>是否存在</returns>
        public bool IsExistCondition(string conditionName)
        {
            return InterceptConditions.ContainsKey(conditionName);
        }
        /// <summary>
        /// 移除拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        public void RemoveInterceptCondition(string conditionName)
        {
            if (InterceptConditions.ContainsKey(conditionName))
            {
                InterceptConditions.Remove(conditionName);
            }
        }
        /// <summary>
        /// 清空所有拦截条件
        /// </summary>
        public void ClearInterceptCondition()
        {
            InterceptConditions.Clear();
        }
    }
}