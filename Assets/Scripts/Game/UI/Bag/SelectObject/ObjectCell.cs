using HT.Framework;
using DG.Tweening;
using UnityEngine.Sprites;
using System.Collections.Generic;
using System;

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

        private List<ObjectsModel> objects => GameManager.Instance.objectsModels;

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
            message.text = objects[Index].Name;
            messageLarge.text = objects[Index].DetailMessage;

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
                ? new Color32(255, 255, 255, 255)
                : new Color32(255, 255, 255, 77);
            var cicomp = imageLarge.gameObject.GetComponent<CreateObject>();
            if (cicomp == null)
                imageLarge.gameObject.AddComponent<CreateObject>().objects = objects[Index];
            else
                cicomp.objects = objects[Index];
            imageLarge.sprite = CommonTools.GetSprite(objects[Index].PreviewImage);
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
