/************************************************************************************
    作者：张峻凡
    描述：测量仪器Cells
*************************************************************************************/
using System.Collections.Generic;
using System;
using System.Linq;

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class InstrumentCell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] Image imageLarge = default;
        [SerializeField] Button button = default;

        //仪器各项内容列表
        //数组长度为仪器数量
        //private string[] message_text = { "直尺", "游标卡尺", "螺旋测微计", "天平", "仪器5", "仪器6" };
        //private string[] messageLarge_text = { "仪器1的描述", "仪器2的描述", "仪器3的描述", "仪器4的描述", "仪器5的描述", "仪器6的描述" };
        //private string image_source = "UI/Resources/Select_instruments/image";
        //private string imageLarge_source = "UI/Resources/Select_instruments/imageLarge";

        private List<Type> instrumentsTypes => CommonTools.GetSubClassNames(typeof(InstrumentBase)).Where(x => !x.IsAbstract).ToList();

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        public override void UpdateContent(ItemData itemData)
        {
            var ins = instrumentsTypes[Index].CreateInstrumentInstance();
            message.text = ins.InstName;
            messageLarge.text = ins.InstName;

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
                ? new Color32(255, 255, 255, 255)
                : new Color32(255, 255, 255, 77);
            //image.sprite = ins.previewImage;
            imageLarge.sprite = ins.previewImage;
            var cicomp = imageLarge.gameObject.GetComponent<CreateInstrument>();
            if (cicomp == null)
                imageLarge.gameObject.AddComponent<CreateInstrument>().InstrumentType = instrumentsTypes[Index];
            else
                cicomp.InstrumentType = instrumentsTypes[Index];
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
