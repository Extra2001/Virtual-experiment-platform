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
            if (RecordManager.tempRecord.objects.Count == 0 || true)
            {
                RecordManager.tempRecord.objects = new List<ObjectsModel>()
                {
                    new ObjectsModel()
                    {
                        id = 0,
                        Name = "立方体",
                        DetailMessage = "纯正立方体",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cubic.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object1.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 1,
                        Name = "圆柱",
                        DetailMessage = "较高的一个圆柱",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cylinder.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object2.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 2,
                        Name = "圆柱",
                        DetailMessage = "较胖的一个圆柱",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/cylinder_low.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object3.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 3,
                        Name = "圆环",
                        DetailMessage = "圆环",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/object4.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object4.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 4,
                        Name = "圆筒",
                        DetailMessage = "带底的一个圆筒",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/object5.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object5.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 5,
                        Name = "正四面体",
                        DetailMessage = "标准的正四面体",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/object6.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object6.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 6,
                        Name = "杯子",
                        DetailMessage = "户外用杯子。可以用来测内径",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/object7.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object7.obj"
                    },
                    new ObjectsModel()
                    {
                        id = 7,
                        Name = "杯盖",
                        DetailMessage = "与杯子匹配的杯盖",
                        Integrated = true,
                        PreviewImage = $"{Application.streamingAssetsPath}/PreviewImages/object8.png",
                        ResourcePath = $"{Application.streamingAssetsPath}/Objects/object8.obj"
                    },
                };
            }
            var items = new ItemData[RecordManager.tempRecord.objects.Count];
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
