using System;
using UnityEngine;

namespace NL
{
    public class TypeObserver<T> : IObserver<T>
    {
        private Action<T> onNext;

        public TypeObserver(Action<T> onNext)
        {
            this.onNext = onNext;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(T value)
        {
            this.onNext(value);
        }
    }
}