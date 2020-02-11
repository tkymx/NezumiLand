using System;
using System.Collections.Generic;

namespace NL {

    public class SelectObservable<T,K> : IObservable<K> {

        private IObservable<T> observable;
        private Func<T,K> operation;

        public SelectObservable (IObservable<T> observable, Func<T,K> operation) {
            this.observable = observable;
            this.operation = operation;
        }

        public IDisposable Subscribe (IObserver<K> observer) {  
            var disposables = new List<IDisposable>();          
            disposables.Add(observable.Subscribe(value => {
                observer.OnNext(operation(value));
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
        public static IObservable<K> Select<T,K>(this IObservable<T> observable, Func<T,K> operation)
        {
            return new SelectObservable<T,K>(observable, operation);
        }
    }
}