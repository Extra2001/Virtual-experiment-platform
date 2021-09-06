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
        Main.m_UI.PreloadingResidentUI<StartUILogic>();
        Main.m_UI.PreloadingResidentUI<UncertainLearnUILogic>();
        Main.m_UI.PreloadingTemporaryUI<PauseUILogic>();
        Main.m_UI.PreloadingTemporaryUI<ModelUILogic>();
        Main.m_UI.PreloadingResidentUI<ChooseExpUILogic>();
        Main.m_UI.PreloadingResidentUI<EnterExpressionUILogic>();
        Main.m_UI.PreloadingResidentUI<AddValueUILogic>();
        Main.m_UI.PreloadingResidentUI<PreviewUILogic>();

        Main.m_UI.PreloadingResidentUI<Indicator1UILogic>();
        Main.m_UI.PreloadingResidentUI<Indicator2UILogic>();
        Main.m_UI.PreloadingResidentUI<Indicator3UILogic>();

        Main.m_UI.PreloadingResidentUI<DatatableUILogic>();
        Main.m_UI.PreloadingTemporaryUI<BagControl>();
        Main.m_UI.PreloadingTemporaryUI<InstrmentInfoUILogic>();

        Main.m_UI.PreloadingResidentUI<ProcessExplain>();
        Main.m_UI.PreloadingResidentUI<MeasuredDataProcess>();
        Main.m_UI.PreloadingResidentUI<ComplexData>();
        Main.m_UI.PreloadingResidentUI<ProcessResult>();
    }
}
