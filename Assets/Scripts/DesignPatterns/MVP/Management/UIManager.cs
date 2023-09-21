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
        private KeyboardListener escapeListener;
        private Presenter current;
        private bool isDisappearing = false;


        private bool initialized;

        public void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            escapeListener = KeyboardListener.Create(nameof(UIManager));
            escapeListener.TargetKeyCode = KeyCode.Escape;
            escapeListener.KeyboardEvent.Subscribe(OnKeyboardEvent);
        }


        public void Navigate<T>(NavigateData data) where T : Presenter, new()
        {
            Presenter presenter = new T();
            presenter.Initialize(data);
            history.Add(presenter);
            current = presenter;
        }

        private void OnKeyboardEvent(KeyboardListener.EventType eventType)
        {
            if (current != null && eventType == KeyboardListener.EventType.Up)
            {
                current.HandleEscapeClose();
            }
        }

        public void Back()
        {
            StartDisappearPresenter();
        }

        private void StartDisappearPresenter()
        {
            if (current == null || history.Count == 0 || isDisappearing)
                return;
            current.Disappear();
            isDisappearing = true;
        }

        public void MarkPresenterDidDisappear(Presenter presenter)
        {
            Debug.Assert(presenter == current);
            history.RemoveAt(history.Count - 1);
            current = null;
            if (history.Count > 0)
            {
                current = history[history.Count - 1];
            }

            isDisappearing = false;
        }
    }
}