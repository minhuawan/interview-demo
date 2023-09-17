using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.Factory
{
    public interface IFacotryItem<T>
    {
        public void OnCreate();
        public void OnActive(T activeParams);
        public void OnInactive();
    }

    public class Factory<T, U> where T : IFacotryItem<U>, new()
    {
        private List<T> activedItems = new List<T>();
        private List<T> inactivedItems = new List<T>();

        public int InactiveCount => inactivedItems.Count;
        public int ActiveCount => activedItems.Count;
        public readonly int WramupCount;

        public Factory(int wramupCount)
        {
            WramupCount = wramupCount;
        }

        public void Wramup()
        {
            while (inactivedItems.Count < WramupCount)
            {
                T item = new T();
                item.OnCreate();
                inactivedItems.Add(item);
            }
        }


        public T Create(U activeParams)
        {
            T item = default;
            if (inactivedItems.Count > 0)
            {
                int index = inactivedItems.Count - 1;
                item = inactivedItems[index];
                inactivedItems.RemoveAt(index);
            }
            else
            {
                item = new T();
                item.OnCreate();
            }

            item.OnActive(activeParams);
            activedItems.Add(item);
            return item;
        }

        public void Release(T item)
        {
            Debug.Assert(activedItems.Contains(item), "activedItems.Contains(item)");
            Debug.Assert(item != null, "item != null");

            if (item != null)
            {
                item.OnInactive();
                inactivedItems.Add(item);
            }
        }
    }
}