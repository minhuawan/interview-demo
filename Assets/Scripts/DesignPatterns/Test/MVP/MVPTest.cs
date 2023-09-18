using System;

namespace DesignPatterns.Test.MVP
{
    public class MVPTest : ITestRunner
    {
        public void Run(object args)
        {
            MyPresenter presenter = new MyPresenter();
            presenter.Initialize();
        }
    }
}