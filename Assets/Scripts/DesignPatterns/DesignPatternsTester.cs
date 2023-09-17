using System.Collections;
using DesignPatterns.Command;
using DesignPatterns.Factory;
using DesignPatterns.Singleton;
using UnityEngine;

namespace DesignPatterns
{
    public class DesignPatternsTester : MonoBehaviour
    {
        private void Start()
        {
        }

        private void TestSingleon()
        {
            Debug.Log("TestSingletonClass.Instance.message: " + TestSingletonClass.Instance.message);
        }


        private Factory<MyCube, MyCube.MyCubeActiveParams> factory;

        private IEnumerator TestFactory()
        {
            factory = new Factory<MyCube, MyCube.MyCubeActiveParams>(wramupCount: 1);
            factory.Wramup();
            MyCube cube1 = factory.Create(new MyCube.MyCubeActiveParams() { color = Color.red, name = "hello" });
            MyCube cube2 = factory.Create(new MyCube.MyCubeActiveParams() { color = Color.blue, name = "world" });

            yield return new WaitForSeconds(2);

            factory.Release(cube1);
            factory.Release(cube2);

            factory.Create(new MyCube.MyCubeActiveParams() { color = Color.yellow, name = "cube" });

            Debug.Assert(factory.WramupCount == 1);
            Debug.Assert(factory.ActiveCount == 1);
            Debug.Assert(factory.InactiveCount == 1);
        }

        private void TestCommand()
        {
            CommandCenter.Register(new CommandTestClass());
            CommandCenter.Execute<CommandTestClass>("Hello, there is TestCommand");
        }
    }


    public class TestSingletonClass : Singleton<TestSingletonClass>
    {
        public string message = "hello world";
    }


    public class MyCube : MonoBehaviour, IFacotryItem<MyCube.MyCubeActiveParams>
    {
        public struct MyCubeActiveParams
        {
            public Color color;
            public string name;
        }

        public void OnCreate()
        {
            Debug.Log("MyCude Created");
            gameObject.SetActive(false);
        }

        public void OnActive(MyCubeActiveParams activeParams)
        {
            // Debug.Log("MyCude OnActive");
            gameObject.name = activeParams.name;
            gameObject.GetComponent<MeshRenderer>().material.color = activeParams.color;
        }

        public void OnInactive()
        {
            gameObject.SetActive(false);
        }
    }

    public class CommandTestClass : Command.Command
    {
        public override void Execute(object args)
        {
            string message = args as string;
            Debug.Log("CommandTestClass receive " + message);
        }
    }
}