/************************************************************************************
    作者：荆煦添
    描述：UI初始化器
*************************************************************************************/
using HT.Framework;

public class UIAPIInitializer : SingletonBase<UIAPIInitializer>
{
    /// <summary>
    /// 初始化UI的API
    /// </summary>
    public void Initialize()
    {
        Main.m_UI.PreloadingUI<StartUILogic>();
        Main.m_UI.PreloadingUI<UncertainLearnUILogic>();
        Main.m_UI.PreloadingUI<PauseUILogic>();
        Main.m_UI.PreloadingUI<ModelUILogic>();
        Main.m_UI.PreloadingUI<SimpleModelPanelUILogic>();
        Main.m_UI.PreloadingUI<ChooseExpUILogic>();
        Main.m_UI.PreloadingUI<EnterExpressionUILogic>();
        Main.m_UI.PreloadingUI<AddValueUILogic>();
        Main.m_UI.PreloadingUI<PreviewUILogic>();

        Main.m_UI.PreloadingUI<Indicator1UILogic>();
        Main.m_UI.PreloadingUI<Indicator2UILogic>();
        Main.m_UI.PreloadingUI<Indicator3UILogic>();

        Main.m_UI.PreloadingUI<DatatableUILogic>();
        Main.m_UI.PreloadingUI<BagControl>();
        Main.m_UI.PreloadingUI<InstrmentInfoUILogic>();

        Main.m_UI.PreloadingUI<ProcessExplain>();
        Main.m_UI.PreloadingUI<MeasuredDataProcess>();
        Main.m_UI.PreloadingUI<ComplexDataUILogic>();
        Main.m_UI.PreloadingUI<ProcessResult>();
    }
}
