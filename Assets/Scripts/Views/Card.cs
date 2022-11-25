using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class Card : MonoBehaviour
    {
        public bool IsHidden => _isHidden;
        private bool _isHidden = true;
    
        [SerializeField] private Image portrait;
        [SerializeField] private Transform frontSide;
        [SerializeField] private Transform backSide;
        [SerializeField] private float flipTime = 0.5f;
        public void SetPortraitTexture(Texture2D texture2D)
        {
            if (texture2D == null)
            {
                portrait.overrideSprite = null;
                return;
            }
            var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            portrait.overrideSprite = sprite;
        }
        public void Show()
        {
            if (!_isHidden) return;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScaleX(0, flipTime).SetEase(Ease.Linear));
            sequence.AppendCallback(() =>
            {
                frontSide.gameObject.SetActive(true);
                backSide.gameObject.SetActive(false);
            });
            sequence.Append(transform.DOScaleX(1, flipTime)).SetEase(Ease.Linear);
            sequence.AppendCallback(() => _isHidden = false);
        }
    
        public void Hide()
        {
            if (_isHidden) return;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScaleX(0, flipTime).SetEase(Ease.Linear));
            sequence.AppendCallback(() =>
            {
                frontSide.gameObject.SetActive(false);
                backSide.gameObject.SetActive(true);
            });
            sequence.Append(transform.DOScaleX(1, flipTime)).SetEase(Ease.Linear);
            sequence.AppendCallback(() => _isHidden = true);
        }
    
    }
}
