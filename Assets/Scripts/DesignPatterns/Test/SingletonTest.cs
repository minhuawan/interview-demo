using UnityEngine;
using DesignPatterns.Singleton;

namespace DesignPatterns.Test
{
    public class SingletonTest : ITestRunner
    {
        public void Run(object args)
        {
            Debug.Log("TestSingletonClass.Instance.message: " + TestSingletonClass.Instance.message);
        }
    }

    public class TestSingletonClass : Singleton<TestSingletonClass>
    {
        public string message = "hello world";
    }
}