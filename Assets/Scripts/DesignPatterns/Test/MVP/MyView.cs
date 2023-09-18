using System;
using DesignPatterns.MVP;
using DesignPatterns.MVP.ViewLoader;
using DesignPatterns.RX;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Test.MVP
{
    [ViewPath("View/MyView")]
    public class MyView : View
    {
        [SerializeField] private Text title;
        [SerializeField] private Button button;
        private Subject<RXVoid> clickSubject = new Subject<RXVoid>();

        public Subject<RXVoid> ClickEvent => clickSubject;

        public override void Appear(Action action)
        {
            button.onClick.AddListener(() => clickSubject.OnNext(RXVoid.Void));
            base.Appear(action);
        }

        public void SetTitle(string titleMsg)
        {
            title.text = titleMsg;
        }
    }
}