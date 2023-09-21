using System;
using DesignPatterns.MVP;
using DesignPatterns.MVP.Animation;
using DesignPatterns.MVP.ViewLoader;
using DesignPatterns.RX;
using UnityEngine;
using UnityEngine.UI;

namespace DesignPatterns.Test.MVP
{
    [ViewPath("View/MyView")]
    public class MyView : AnimationView
    {
        [SerializeField] private Text title;
        [SerializeField] private Button button;
        private Subject<RXVoid> clickSubject = new Subject<RXVoid>();

        public ISubject<RXVoid> ClickEvent => clickSubject;

        public override void Appear(Action action)
        {
            disposables.Add(clickSubject);
            button.onClick.AddListener(() => clickSubject.OnNext(RXVoid.Void));
            base.Appear(action);
        }

        public void SetTitle(string titleMsg)
        {
            title.text = titleMsg;
        }

        public void SetButtonActive(bool active)
        {
            button.gameObject.SetActive(active);
        }
    }
}