using System.Linq;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_instrument : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = CommonTools.GetSubClassNames(typeof(InstrumentBase))
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
