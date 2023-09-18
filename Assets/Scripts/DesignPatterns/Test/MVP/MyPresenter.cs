using DesignPatterns.Extension;
using DesignPatterns.MVP;

namespace DesignPatterns.Test.MVP
{
    public class MyPresenter : Presenter
    {
        private int clickedTimes = 0;

        private MyView myView = null;
        public override View view => myView;

        public override void Initialize()
        {
            base.Initialize();
            View.LoadView<MyView>(this);
        }

        public override void OnViewLoaded(View loadedView)
        {
            myView = (MyView)loadedView;
            myView.SetTitle("");
            myView.ClickEvent.Subscribe(_ => OnButtonClicked()).AddTo(disposables);
            Appear();
        }

        protected override void OnDidAppear()
        {
            myView.SetTitle("Try to click the button");
        }

        protected override void OnDidDisappear()
        {
            Dispose();
        }

        private void OnButtonClicked()
        {
            clickedTimes++;
            string msg = $"You clicked the Button {clickedTimes} times" +
                         $"\nClick it over 5 times to disappear the view";
            myView.SetTitle(msg);
            if (clickedTimes == 5)
            {
                Disappear();
                Dispose();
            }
        }
    }
}