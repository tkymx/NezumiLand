using System.Collections.Generic;
using System;

namespace NL
{
    public class ActionDisposer : IDisposable
    {
        private Action dispose;

        public ActionDisposer(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            this.dispose();
        }
    }

    public class TypeObservable<T> : IObservable<T>
    {
        List<IObserver<T>> observers;

        public TypeObservable()
        {
            this.observers = new List<IObserver<T>>();
        }

        public void Execute(T value)
        {
            this.observers.ForEach(observer => observer.OnNext(value));
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observers.Add(observer);
            return new ActionDisposer(() =>
            {
                this.observers.Remove(observer);
            });
        }

        // ちょっと特殊
        public IDisposable Subscribe(Action<T> action)
        {
            IObserver<T> observer = new TypeObserver<T>(action);
            observers.Add(observer);
            return new ActionDisposer(() =>
            {
                this.observers.Remove(observer);
            });
        }
    }
}