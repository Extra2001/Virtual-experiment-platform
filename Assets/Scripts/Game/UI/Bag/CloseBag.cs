/************************************************************************************
    作者：张峻凡
    描述：关闭抽屉的处理程序
*************************************************************************************/
using HT.Framework;
using UnityEngine.UI;

public class CloseBag : HTBehaviour
{
    /// <summary>
    /// 订阅点击事件
    /// </summary>
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(CloseIt);
    }
    /// <summary>
    /// 关闭BagUI
    /// </summary>
    private void CloseIt()
    {
        Main.m_UI.GetUI<BagControl>().Hide();
    }
}
