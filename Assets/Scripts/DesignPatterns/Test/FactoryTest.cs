using System.Collections;
using DesignPatterns.Factory;
using UnityEngine;

namespace DesignPatterns.Test
{
    public class FactoryTest : ITestRunner
    {
        private Factory<MyCube, MyCube.MyCubeActiveParams> factory;

        public IEnumerator Start()
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

        public void Run(object args)
        {
            GameObject go = new GameObject(nameof(FactoryTest));
            FactoryMonoWrapper monoWrapper = go.AddComponent<FactoryMonoWrapper>();
            monoWrapper.StartCoroutine(Start());
        }
    }

    class FactoryMonoWrapper : MonoBehaviour
    {
    }


    class MyCube : MonoBehaviour, IFacotryItem<MyCube.MyCubeActiveParams>
    {
        public struct MyCubeActiveParams
        {
            public Color color;
            public string name;
        }

        public void OnCreate()
        {
            Debug.Log("MyCube Created");
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
}