using System;
using System.Collections;
using System.Linq;
using ImageLoader;
using UnityEngine;
using Views;

namespace CardDisplayMethods
{
    public class WhenReady : ICardDisplayMethod
    {
        public event Action DisplayFinished;
        private readonly Card[] _cards;
        private readonly IImageLoader<Texture2D> _textureLoader;

        public WhenReady(IImageLoader<Texture2D> textureLoader, Card[] cards)
        {
            _textureLoader = textureLoader;
            _cards = cards;
        }
        
        public IEnumerator DisplayRoutine()
        {
            foreach (var card in _cards)
            {
                card.StartCoroutine(
                    _textureLoader.LoadRoutine(texture2D =>
                    {
                        card.SetPortraitTexture(texture2D);
                        card.Show();
                    }));
            }
            
            yield return new WaitUntil(() => _cards.All(c => !c.IsHidden));
            DisplayFinished?.Invoke();
        }

    }
}