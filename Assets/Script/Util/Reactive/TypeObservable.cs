using System;
using System.Collections.Generic;

namespace NL {
    public class ActionDisposer : IDisposable {
        private Action dispose;

        public ActionDisposer (Action dispose) {
            this.dispose = dispose;
        }

        public void Dispose () {
            this.dispose ();
        }
    }

    public class TypeObservable<T> : IObservable<T> {
        List<IObserver<T>> observers;

        public TypeObservable () {
            this.observers = new List<IObserver<T>> ();
        }

        public void Execute (T value) {
            var tempObservers = this.observers.ToArray();
            foreach (var observer in tempObservers)
            {
                observer.OnNext (value);
            }
        }

        public IDisposable Subscribe (IObserver<T> observer) {
            observers.Add (observer);
            return new ActionDisposer (() => {
                this.observers.Remove (observer);
            });
        }
    }

    public class ImmediatelyObservable<T> : IObservable<T> {

        T value;

        public ImmediatelyObservable (T value) {
            this.value = value;
        }

        public IDisposable Subscribe (IObserver<T> observer) {
            observer.OnNext(this.value);
            return new ActionDisposer (() => {
            });
        }
    }    

    public static partial class IObservableExtension
    {
        // SubScribe の一般化
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> action)
        {
            return observable.Subscribe(new TypeObserver<T>(value => action(value)));
        }
    }
}