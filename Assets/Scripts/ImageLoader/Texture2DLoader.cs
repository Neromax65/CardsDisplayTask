using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ImageLoader
{
    public class Texture2DLoader : IImageLoader<Texture2D>
    {
        private readonly string _url;
        
        public Texture2DLoader(string url)
        {
            _url = url;
        }
        
        public IEnumerator LoadRoutine(Action<Texture2D> downloadFinished)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(_url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    downloadFinished?.Invoke(texture);
                }
                else
                {
                    Debug.LogError(webRequest.error);
                    downloadFinished?.Invoke(null);
                }
            }
        }
    }
}