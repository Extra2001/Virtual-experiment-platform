using HT.Framework;

[UIResource(null, null, "UI/TipsPanel")]
public class TipsUILogic : UILogicTemporary
{
    public override void OnOpen(params object[] args)
    {
        base.OnOpen(args);

        if (args.Length == 3)
        {
            UIEntity.GetComponent<TipsPanel>().Show(args[0] as string, args[1] as string, (float)args[2]);
        }
    }
}
