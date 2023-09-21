using DesignPatterns.Extension;
using DesignPatterns.MVP.Management;
using UnityEngine;

namespace DesignPatterns.MVP
{
    public abstract class Presenter : DisposableContainer
    {
        protected abstract View view { get; }

        protected NavigateData navigateData;


        public virtual void Initialize(NavigateData navigateData)
        {
            this.navigateData = navigateData;
        }

        public abstract void OnViewLoaded(View loadedView);

        public virtual void Appear()
        {
            view.Appear(OnDidAppear);
        }

        protected virtual void OnDidAppear()
        {
        }

        public virtual void Disappear()
        {
            view.Disappear(OnDidDisappear);
        }

        protected virtual void OnDidDisappear()
        {
            UIManager.Instance.MarkPresenterDidDisappear(this);
            Dispose();
        }

        public override void Dispose()
        {
            if (disposed)
                return;

            Debug.Log($"{GetType().FullName} Disposed");

            if (view != null)
            {
                view.Dispose();
            }

            base.Dispose(); // dispose container
        }

        public virtual void HandleEscapeClose()
        {
            UIManager.Instance.Back();
        }
    }
}