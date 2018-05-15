using System;
using System.Collections.Generic;
using System.Reactive;
using Microsoft.Reactive.Testing;

namespace NUnit.Rx
{
	public class MockObserver<T> : ITestableObserver<T>
	{
		TestScheduler scheduler;
		readonly List<Recorded<Notification<T>>> messages;

		public MockObserver(TestScheduler scheduler)
		{
			this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
			messages = new List<Recorded<Notification<T>>>();
		}

		public void OnNext(T value) => messages.Add(new Recorded<Notification<T>>(scheduler.Clock, Notification.CreateOnNext<T>(value)));

		public void OnError(Exception exception) => messages.Add(new Recorded<Notification<T>>(scheduler.Clock, Notification.CreateOnError<T>(exception)));

		public void OnCompleted() => messages.Add(new Recorded<Notification<T>>(scheduler.Clock, Notification.CreateOnCompleted<T>()));

		public IList<Recorded<Notification<T>>> Messages => messages;
	}
}
