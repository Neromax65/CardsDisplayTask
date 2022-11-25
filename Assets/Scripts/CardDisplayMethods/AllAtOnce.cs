using System;
using System.Collections;
using System.Linq;
using ImageLoader;
using UnityEngine;
using Views;

namespace CardDisplayMethods
{
    public class AllAtOnce : ICardDisplayMethod
    {
        public event Action DisplayFinished;
        private readonly IImageLoader<Texture2D> _textureLoader;
        private readonly Card[] _cards;
        private readonly string _url;

        public AllAtOnce(IImageLoader<Texture2D> textureLoader, Card[] cards)
        {
            _textureLoader = textureLoader;
            _cards = cards;
        }
        
        public IEnumerator DisplayRoutine()
        {
            int portraitSettedCount = 0;
            foreach (var card in _cards)
            {
                card.StartCoroutine(
                    _textureLoader.LoadRoutine(texture2D =>
                    {
                        card.SetPortraitTexture(texture2D);
                        portraitSettedCount++;
                    }));
            }
            yield return new WaitUntil(() => portraitSettedCount >= _cards.Length);

            foreach (var card in _cards)
                card.Show();

            yield return new WaitUntil(() => _cards.All(c => !c.IsHidden));
            DisplayFinished?.Invoke();
        }

    }
}