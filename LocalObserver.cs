using System;
namespace NUnit.Rx
{
	public class LocalObserver<T> : IObserver<T>
	{
		private Action<T> action;

		public LocalObserver(Action<T> action)
		{
			this.action = action;
		}

		public void OnCompleted()
		{
}

		public void OnError(Exception error)
		{
		}

		public void OnNext(T value)
		{
			this.action?.Invoke(value);
		}
	}
}
