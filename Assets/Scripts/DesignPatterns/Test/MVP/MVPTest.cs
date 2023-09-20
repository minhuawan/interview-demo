using System;
using System.Collections.Generic;
using DesignPatterns.MVP.Management;

namespace DesignPatterns.Test.MVP
{
    public class MVPTest : ITestRunner
    {
        public void Run(object args)
        {
            NavigateData navigateData = new NavigateData(new Dictionary<string, object>()
            {
                [MyPresenter.Content] = "Click button to create another view",
                [MyPresenter.ClickToClose] = true,
            });
            UIManager.Instance.Navigate<MyPresenter>(navigateData);
        }
    }
}