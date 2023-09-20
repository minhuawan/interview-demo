using System.Collections.Generic;
using DesignPatterns.Singleton;
using UnityEngine;

namespace DesignPatterns.MVP.Management
{
    public struct NavigateData
    {
        public readonly IDictionary<string, object> InitialDataSet;

        public NavigateData(IDictionary<string, object> initialDataSet)
        {
            InitialDataSet = initialDataSet;
        }

        public T GetData<T>(string key)
        {
            if (InitialDataSet.TryGetValue(key, out object oObject))
            {
                return (T)oObject;
            }

            return default(T);
        }

        public bool HasData(string key)
        {
            return InitialDataSet.ContainsKey(key);
        }
    }

    public class UIManager : Singleton<UIManager>
    {
        private List<Presenter> history = new List<Presenter>();
        private Presenter current = null;
        private KeyboardListener escapeListener;


        private bool initialized;

        public void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            escapeListener = KeyboardListener.Create(nameof(UIManager));
            escapeListener.TargetKeyCode = KeyCode.Escape;
            escapeListener.KeyboardEvent.Subscribe(_ => Back());
        }

        public void Navigate<T>(NavigateData data) where T : Presenter, new()
        {
            Presenter presenter = new T();
            presenter.Initialize(data);
            history.Add(presenter);
            current = presenter;
        }

        public void Back()
        {
            if (history.Count == 0)
                return;
            int index = history.Count - 1;
            Presenter presenter = history[index];
            presenter.Disappear();
            history.RemoveAt(index);
        }
    }
}