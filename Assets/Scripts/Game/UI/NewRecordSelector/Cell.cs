using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


namespace UI.NewRecordSelector
{
    class Cell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] Image imageLarge = default;
        [SerializeField] Button button = default;
        [SerializeField] Button confirmButton = default;
        [SerializeField] Text time = default;
        [SerializeField] InputField recordName = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));

            confirmButton.onClick.AddListener(() =>
            {
                Debug.Log($"已确定存档{Index}, 存档名称{recordName.text}");
            });
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.RecordName;
            messageLarge.text = itemData.RecordName;
            time.text = itemData.Time;

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
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
