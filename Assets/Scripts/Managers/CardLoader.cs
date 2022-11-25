using System;
using System.Collections;
using System.Linq;
using CardDisplayMethods;
using ImageLoader;
using TMPro;
using UnityEngine;
using Views;
using Button = UnityEngine.UI.Button;

namespace Managers
{
    public class CardLoader : MonoBehaviour
    {
        [SerializeField] private string pictureURL = "https://picsum.photos/200/300";

        [SerializeField] private Card[] cards;
        [SerializeField] private TMP_Dropdown modeDropdown; 
        [SerializeField] private Button loadButton;
        [SerializeField] private Button cancelButton;
        private IImageLoader<Texture2D> _textureLoader;
        private DisplayMode _currentMode;
        private Coroutine _showRoutine;

        private enum DisplayMode
        {
            OneByOne,
            AllAtOnce,
            ImmediatelyWhenReady
        }

        private void Awake()
        {
            _textureLoader = new Texture2DLoader(pictureURL);
            modeDropdown.onValueChanged.AddListener(OnModeChanged);
            loadButton.onClick.AddListener(OnLoadButtonClick);
            cancelButton.onClick.AddListener(OnCancelButtonClick);
        }

        private void OnModeChanged(int mode)
        {
            _currentMode = (DisplayMode)mode;
        }
    

        private void OnCancelButtonClick()
        {
            if (_showRoutine != null)
                StopCoroutine(_showRoutine);
            foreach (var card in cards)
                card.StopAllCoroutines();
            loadButton.interactable = true;
            cancelButton.interactable = false;
        }

        private IEnumerator HideCards(Action callback)
        {
            foreach (var card in cards)
                card.Hide();

            yield return new WaitUntil(() => cards.All(c => c.IsHidden));
            callback?.Invoke();
        }

        private void OnLoadButtonClick()
        {
            if (_showRoutine != null)
                StopCoroutine(_showRoutine);
            foreach (var card in cards)
                card.StopAllCoroutines();
            loadButton.interactable = false;
            cancelButton.interactable = true;
            StartCoroutine(HideCards(() =>
            {
                ICardDisplayMethod cardDisplayMethod = _currentMode switch
                {
                    DisplayMode.OneByOne => new OneByOne(_textureLoader, cards, 0.2f),
                    DisplayMode.AllAtOnce => new AllAtOnce(_textureLoader, cards),
                    DisplayMode.ImmediatelyWhenReady => new WhenReady(_textureLoader, cards),
                    _ => throw new ArgumentOutOfRangeException()
                };
                cardDisplayMethod.DisplayFinished += () =>
                {
                    loadButton.interactable = true;
                    cancelButton.interactable = false;
                };
                _showRoutine = StartCoroutine(cardDisplayMethod.DisplayRoutine());
            }));
        }
    }
}
