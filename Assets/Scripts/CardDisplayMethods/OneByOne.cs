using System;
using System.Collections;
using System.Linq;
using ImageLoader;
using UnityEngine;
using Views;

namespace CardDisplayMethods
{
    public class OneByOne : ICardDisplayMethod
    {
        public event Action DisplayFinished;
        private readonly Card[] _cards;
        private readonly IImageLoader<Texture2D> _textureLoader;
        private readonly float _delay;

        public OneByOne(IImageLoader<Texture2D> textureLoader, Card[] cards, float delay)
        {
            _textureLoader = textureLoader;
            _cards = cards;
            _delay = delay;
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
            {
                card.Show();
                yield return new WaitForSeconds(_delay);
            }
            
            yield return new WaitUntil(() => _cards.All(c => !c.IsHidden));
            DisplayFinished?.Invoke();
        }

    }
}