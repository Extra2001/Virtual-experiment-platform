/************************************************************************************
    作者：张峻凡
    描述：显示可选的被测物体们
*************************************************************************************/
using HT.Framework;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_object : HTBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = new ItemData[GameManager.Instance.objectsModels.Count];
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
