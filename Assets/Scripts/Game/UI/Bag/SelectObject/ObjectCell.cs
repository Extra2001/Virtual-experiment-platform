using HT.Framework;
using DG.Tweening;
using UnityEngine.Sprites;
namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample03
{
    class ObjectCell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] Image imageLarge = default;
        [SerializeField] Button button = default;

        //仪器各项内容列表
        //数组长度为仪器数量
        private string[] message_text = { "物体1", "物体2", "物体3", "物体4", "物体5", "物体6" };
        private string[] messageLarge_text = { "被测物体1的描述", "被测物体2的描述", "被测物体3的描述", "被测物体4的描述", "被测物体5的描述", "被测物体6的描述" };
        private string image_source = "UI/Resources/Select_instruments/image";
        private string imageLarge_source = "UI/Resources/Select_instruments/imageLarge";


        //

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
            message.text = message_text[Index];
            messageLarge.text = messageLarge_text[Index];

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
                ? new Color32(255, 255, 255, 255)
                : new Color32(255, 255, 255, 77);

            image.sprite = Resources.Load<Sprite>(image_source + (Index + 1));
            imageLarge.sprite = Resources.Load<Sprite>(imageLarge_source + (Index + 1));

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

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
