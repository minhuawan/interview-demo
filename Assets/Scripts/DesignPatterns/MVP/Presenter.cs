using DesignPatterns.Extension;
using UnityEngine;

namespace DesignPatterns.MVP
{
    public abstract class Presenter : DisposableContainer
    {
        public abstract View view { get; }


        public virtual void Initialize()
        {
        }

        public abstract void OnViewLoaded(View loadedView);

        public virtual void Appear()
        {
            view.Appear(OnDidAppear);
        }

        protected abstract void OnDidAppear();

        public virtual void Disappear()
        {
            view.Disappear(OnDidDisappear);
        }

        protected abstract void OnDidDisappear();

        public override void Dispose()
        {
            if (view != null)
            {
                view.Dispose();
            }

            base.Dispose();
        }
    }
}