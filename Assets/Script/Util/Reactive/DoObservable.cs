using System;
using System.Collections.Generic;

namespace NL {

    public class DoObservable<T> : IObservable<T> {

        private IObservable<T> observable;
        private Action<T> operation;

        public DoObservable (IObservable<T> observable, Action<T> operation) {
            this.observable = observable;
            this.operation = operation;
        }

        public IDisposable Subscribe (IObserver<T> observer) {
            var disposables = new List<IDisposable>();          
            disposables.Add(observable.Subscribe(value => {
                this.operation(value);
                observer.OnNext(value);
            }));

            return new ActionDisposer (() => {
                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
            });
        }
    }

    public static partial class IObservableExtension
    {
        // 結果をまつ Obserbable
        public static IObservable<T> Do<T>(this IObservable<T> observable, Action<T> operation)
        {
            return new DoObservable<T>(observable, operation);
        }
    }
}