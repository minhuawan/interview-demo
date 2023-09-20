using System;
using DesignPatterns.RX;
using UnityEngine;

namespace DesignPatterns.MVP.Management
{
    public class KeyboardListener : MonoBehaviour, IDisposable
    {
        public static KeyboardListener Create(string name)
        {
            GameObject go = new GameObject(nameof(KeyboardListener) + name);
            KeyboardListener listener = go.AddComponent<KeyboardListener>();
            DontDestroyOnLoad(go);
            return listener;
        }

        public enum EventType
        {
            Down,
            Up,
        }

        public KeyCode TargetKeyCode = KeyCode.Escape;
        public ISubject<EventType> KeyboardEvent => subject;

        private Subject<EventType> subject = new Subject<EventType>();
        private bool pressed;
        private bool disposed;


        private void Update()
        {
            if (disposed)
                return;

            if (!pressed && Input.GetKeyDown(TargetKeyCode))
            {
                pressed = true;
                subject.OnNext(EventType.Down);
            }

            if (pressed && Input.GetKeyUp(TargetKeyCode))
            {
                pressed = false;
                subject.OnNext(EventType.Up);
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;
            subject.Dispose();
            Destroy(this);
        }
    }
}