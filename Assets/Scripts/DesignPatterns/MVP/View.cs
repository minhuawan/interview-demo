using System;
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

        protected virtual void Awake()
        {
        }


        public virtual void Appear(Action action)
        {
            gameObject.SetActive(true);
            action();
        }

        public virtual void Disappear(Action action)
        {
            gameObject.SetActive(false);
            action();
        }

        public void Dispose()
        {
        }
    }

    public abstract class AnimateView : View
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Appear(Action action)
        {
            base.Appear(action);
        }

        public override void Disappear(Action action)
        {
            base.Disappear(action);
        }
    }

    public class AnimateViewHelper
    {
        
    }
}