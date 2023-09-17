namespace DesignPatterns.Singleton
{
    public abstract class Singleton<T> where T : new()
    {
        private static readonly object locker = new object();
        private static T _instance_;

        public static T Instance
        {
            get
            {
                if (_instance_ == null)
                {
                    lock (locker)
                    {
                        _instance_ = new T();
                    }
                }

                return _instance_;
            }
        }
    }

    public class SingletonTest : Singleton<SingletonTest>
    {
        
    }
}