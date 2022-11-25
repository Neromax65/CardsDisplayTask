using System;
using System.Collections;

namespace CardDisplayMethods
{
    public interface ICardDisplayMethod
    {
        IEnumerator DisplayRoutine();

        event Action DisplayFinished;
    }
}