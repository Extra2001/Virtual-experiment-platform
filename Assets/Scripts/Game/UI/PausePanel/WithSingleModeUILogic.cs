using HT.Framework;

public abstract class WithSingleModeUILogic : UILogicTemporary
{
    private bool singleMode = false;

    public override void OnOpen(params object[] args)
    {
        if (Main.Current.Pause == false)
        {
            PauseManager.Instance.Pause();
            singleMode = true;
        }
        else singleMode = false;

        base.OnOpen(args);
    }

    public override void OnClose()
    {
        if (singleMode) PauseManager.Instance.Continue();

        base.OnClose();
    }
}

