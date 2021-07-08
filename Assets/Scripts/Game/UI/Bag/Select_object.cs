using HT.Framework;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class Select_object : HTBehaviour
    {
        //启用自动化
        protected override bool IsAutomate => true;

        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = new ItemData[GameManager.Instance.objectsModels.Count];
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
