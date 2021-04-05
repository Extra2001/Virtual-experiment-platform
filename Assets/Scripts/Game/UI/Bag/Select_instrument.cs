using System.Linq;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_instrument : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = new ItemData[CommonTools.GetSubClassNames(typeof(InstrumentBase)).Count]; 
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
