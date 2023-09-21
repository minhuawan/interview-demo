using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DesignPatterns.Extension;
using DesignPatterns.MVP.ViewLoader;
using UnityEngine;

namespace DesignPatterns.MVP
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        private static readonly IViewLoader viewLoader = new EditorViewLoader();


        public static void LoadView<T>(Presenter presenter) where T : View
        {
            viewLoader.LoadView<T>(view => { presenter.OnViewLoaded(view); });
        }

        protected bool disposed;
        protected List<IDisposable> disposables = new List<IDisposable>();


        public virtual void Appear(Action action)
        {
            gameObject.SetActive(true);
            action();
        }

        public virtual void Disappear(Action action)
        {
            action();
            gameObject.SetActive(false);
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            for (int i = 0; i < disposables.Count; i++)
            {
                disposables[i].Dispose();
            }

            disposables.Clear();
            UnityEngine.GameObject.Destroy(gameObject);
            
            Debug.Log($"{GetType().FullName} Disposed");
        }
    }



    
}