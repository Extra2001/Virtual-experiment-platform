using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HT.Framework
{
    /// <summary>
    /// 切面代理追踪器
    /// </summary>
    [DisallowMultipleComponent]
    [InternalModule(HTFrameworkModule.AspectTrack)]
    public sealed class AspectTracker : InternalModuleBase
    {
        /// <summary>
        /// 是否启用切面追踪【请勿在代码中修改】
        /// </summary>
        [SerializeField] internal bool IsEnableAspectTrack = false;
        /// <summary>
        /// 是否启用全局拦截（注意：只拦截无返回值方法的调用）
        /// </summary>
        public bool IsEnableIntercept = false;
        
        private IAspectTrackHelper _helper;

        /// <summary>
        /// 全局拦截条件
        /// </summary>
        internal Dictionary<string, HTFFunc<MethodBase, object[], bool>> InterceptConditions
        {
            get
            {
                return _helper.InterceptConditions;
            }
        }

        private AspectTracker()
        {

        }
        internal override void OnInitialization()
        {
            base.OnInitialization();

            _helper = Helper as IAspectTrackHelper;
        }

        /// <summary>
        /// 新增拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <param name="interceptCondition">拦截规则</param>
        public void AddInterceptCondition(string conditionName, HTFFunc<MethodBase, object[], bool> interceptCondition)
        {
            _helper.AddInterceptCondition(conditionName, interceptCondition);
        }
        /// <summary>
        /// 是否存在拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        /// <returns>是否存在</returns>
        public bool IsExistCondition(string conditionName)
        {
            return _helper.IsExistCondition(conditionName);
        }
        /// <summary>
        /// 移除拦截条件
        /// </summary>
        /// <param name="conditionName">条件名称</param>
        public void RemoveInterceptCondition(string conditionName)
        {
            _helper.RemoveInterceptCondition(conditionName);
        }
        /// <summary>
        /// 清空所有拦截条件
        /// </summary>
        public void ClearInterceptCondition()
        {
            _helper.ClearInterceptCondition();
        }
    }
}