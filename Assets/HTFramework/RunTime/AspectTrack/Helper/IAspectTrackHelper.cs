using System.Collections.Generic;
using System.Reflection;

namespace HT.Framework
{
    /// <summary>
    /// 切面代理追踪器的助手接口
    /// </summary>
    public interface IAspectTrackHelper : IInternalModuleHelper
    {
        /// <summary>
        /// 所有的代理对象【真实对象、代理对象】
        /// </summary>
        Dictionary<IAspectTrackObject, IAspectTrackObject> ProxyObjects { get; }
        /// <summary>
        /// 所有的代理者【真实对象，代理者】
        /// </summary>
        Dictionary<IAspectTrackObject, object> Proxys { get; }
        /// <summary>
        /// 全局拦截条件
        /// </summary>
        Dictionary<string, HTFFunc<MethodBase, object[], bool>> InterceptConditions { get; }

        /// <summary>
        /// 新增拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <param name="interceptCondition">拦截规则</param>
        void AddInterceptCondition(string conditionName, HTFFunc<MethodBase, object[], bool> interceptCondition);
        /// <summary>
        /// 是否存在拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <returns>是否存在</returns>
        bool IsExistCondition(string conditionName);
        /// <summary>
        /// 移除拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        void RemoveInterceptCondition(string conditionName);
        /// <summary>
        /// 清空所有拦截条件
        /// </summary>
        void ClearInterceptCondition();
    }
}