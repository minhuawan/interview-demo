using System.Collections.Generic;
using DesignPatterns.MVP;
using DesignPatterns.MVP.Management;

namespace DesignPatterns.Test.MVP
{
    public class MyPresenter : Presenter
    {
        public static string Content = "ContentKey";
        public static string ClickToClose = "ClickToClose";
        public static string EscapeToClose = "EscapeToClose";

        public override void HandleEscapeClose()
        {
            if (navigateData.HasData(EscapeToClose))
            {
                base.HandleEscapeClose();   
            }
        }

        private int clickedTimes = 0;

        private MyView myView = null;
        protected override View view => myView;

        public override void Initialize(NavigateData data)
        {
            base.Initialize(data);
            View.LoadView<MyView>(this);
        }

        public override void OnViewLoaded(View loadedView)
        {
            myView = (MyView)loadedView;
            myView.ClickEvent.Subscribe(_ => OnButtonClicked());
            myView.SetTitle(navigateData.GetData<string>(Content));
            myView.SetButtonActive(navigateData.HasData(ClickToClose));
            Appear();
        }

        private void OnButtonClicked()
        {
            if (navigateData.HasData(ClickToClose))
            {
                clickedTimes++;
                if (clickedTimes == 1)
                {
                    CreateAnother();
                }
                else
                {
                    string msg = $"\nClicked  {clickedTimes} times" +
                                 $"\nOver 5 times. view will disappear";
                    myView.SetTitle(msg);
                    if (clickedTimes >= 6)
                    {
                        Disappear();
                        Dispose();
                    }
                }
            }
        }

        private void CreateAnother()
        {
            NavigateData newData = new NavigateData(new Dictionary<string, object>()
            {
                [Content] = "Type `Esc` to close",
            });
            UIManager.Instance.Navigate<MyPresenter>(newData);
        }
    }
}