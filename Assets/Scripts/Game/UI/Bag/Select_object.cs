using HT.Framework;
using DG.Tweening;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_object : HTBehaviour
    {
    //启用自动化
    protected override bool IsAutomate => true;
    
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = RecordManager.tempRecord.objects
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();
            Debug.Log(items.Length);
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
