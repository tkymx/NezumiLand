using System;
using System.Collections.Generic;

namespace NL {

    public class SelectManyObservable<T> : IObservable<T> {

        private IObservable<T> observable;
        private Func<T,IObservable<T>> operation;

        public SelectManyObservable (IObservable<T> observable, Func<T,IObservable<T>> operation) {
            this.observable = observable;
            this.operation = operation;
        }

        public IDisposable Subscribe (IObserver<T> observer) {  
            var disposables = new List<IDisposable>();          
            disposables.Add(observable.Subscribe(value1 => {
                disposables.Add(operation(value1).Subscribe(value2 => {
                    observer.OnNext(value2);
                }));
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
        public static IObservable<T> SelectMany<T>(this IObservable<T> observable, Func<T,IObservable<T>> operation)
        {
            return new SelectManyObservable<T>(observable, operation);
        }
    }
}