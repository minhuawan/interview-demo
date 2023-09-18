using System;
using System.Collections.Generic;
using DesignPatterns.Test;
using DesignPatterns.Test.MVP;
using UnityEngine;

namespace DesignPatterns
{
    public class DesignPatternsTester : MonoBehaviour
    {
        public enum TestTarget
        {
            Singleton,
            Factory,
            Command,
            RX,
            MVP,
        }

        [SerializeField] private TestTarget target;

        private readonly Dictionary<TestTarget, ITestRunner> testRunners = new Dictionary<TestTarget, ITestRunner>()
        {
            [TestTarget.Singleton] = new SingletonTest(),
            [TestTarget.Factory] = new FactoryTest(),
            [TestTarget.Command] = new CommandTest(),
            [TestTarget.RX] = new RXTest(),
            [TestTarget.MVP] = new CommandTest(),
            [TestTarget.MVP] = new MVPTest(),
        };

        private void Start()
        {
            if (testRunners.TryGetValue(target, out ITestRunner runner))
            {
                runner.Run(null);
            }
        }
    }
}