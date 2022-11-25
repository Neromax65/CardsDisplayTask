using System;
using System.Collections;

namespace ImageLoader
{
    public interface IImageLoader<out T> 
    {
        IEnumerator LoadRoutine(Action<T> downloadFinished);
    }
}