using System.Collections.Generic;

namespace HT.Framework
{
    /// <summary>
    /// 非常驻UI
    /// </summary>
    public abstract class UILogicTemporary : UILogicBase
    {
        internal UILogicTemporary _lastUILogic = null;

        public void NavigateBack()
        {
            Main.m_UI.NavigateBackTemporaryUI();
        }

        /// <summary>
        /// 打开自己
        /// </summary>
        protected override void Open()
        {
            base.Open();

            Main.m_UI.OpenTemporaryUI(GetType());
        }

        /// <summary>
        /// 关闭自己
        /// </summary>
        protected override void Close()
        {
            base.Close();

            Main.m_UI.CloseUI(GetType());
        }
    }
}
