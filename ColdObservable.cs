using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using Microsoft.Reactive.Testing;

namespace NUnit.Rx
{
	public class ColdObservable<T> : ITestableObservable<T>
    {
        readonly TestScheduler scheduler;
        readonly Recorded<Notification<T>>[] messages;
        readonly List<Subscription> subscriptions = new List<Subscription>();

        public ColdObservable(TestScheduler scheduler, params Recorded<Notification<T>>[] messages)
        {
			this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            this.messages = messages ?? throw new ArgumentNullException(nameof(messages));
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            subscriptions.Add(new Subscription(scheduler.Clock));
            var index = subscriptions.Count - 1;

            var d = new CompositeDisposable();

            for (var i = 0; i < messages.Length; ++i)
            {
                var notification = messages[i].Value;
                d.Add(scheduler.ScheduleRelative(default(object), messages[i].Time, (scheduler1, state1) => { notification.Accept(observer); return Disposable.Empty; }));
            }

            return Disposable.Create(() =>
            {
                subscriptions[index] = new Subscription(subscriptions[index].Subscribe, scheduler.Clock);
                d.Dispose();
            });
        }

		public IList<Subscription> Subscriptions => subscriptions;

		public IList<Recorded<Notification<T>>> Messages => messages;
	}
}
