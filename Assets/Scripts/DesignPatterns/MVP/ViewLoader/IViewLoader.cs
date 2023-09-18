using System;

namespace DesignPatterns.MVP.ViewLoader
{
    public interface IViewLoader
    {
        public void LoadView<T>(Action<T> action) where T : View;
    }
}