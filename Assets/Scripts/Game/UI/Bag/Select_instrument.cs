/************************************************************************************
    作者：张峻凡
    描述：显示可选的仪器们
*************************************************************************************/
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_instrument : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = new ItemData[CommonTools.GetSubClassNames(typeof(InstrumentBase))
                .Where(x => !x.IsAbstract).Count()];
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
