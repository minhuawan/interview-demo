using System;

namespace DesignPatterns.RX
{
    public class Subscription<T> : IDisposable
    {
        public delegate void function(T t);

        protected Subject<T> subject;
        protected function func;
        private bool disposed;

        public Subscription(Subject<T> subject, function func)
        {
            this.subject = subject;
            this.func = func;
        }

        public void OnNext(T t)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Subscription<T>));
            }

            func(t);
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            if (subject != null)
            {
                subject.Unsubscribe(this);
            }

            subject = null;
            func = null;
        }
    }
}