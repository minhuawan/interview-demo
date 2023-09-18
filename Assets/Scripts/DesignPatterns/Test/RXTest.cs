using System;
using DesignPatterns.RX;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DesignPatterns.Test
{
    public class RXTest : ITestRunner
    {
        public void Run(object args)
        {
            RXTestClass.RXTestPresenter presenter = new RXTestClass.RXTestPresenter();
            presenter.Initialize();
            presenter.ClickOver5TimesEvent.Subscribe(_ => { presenter.Dispose(); });
        }
    }

    public class RXTestClass
    {
        public class RXTestView : MonoBehaviour
        {
            [SerializeField] private Button button;
            [SerializeField] private Text content;
            private Subject<int> clickSubject = new Subject<int>();
            public ISubject<int> ClickEvent => clickSubject;

            private int clickCounter;

            private void Start()
            {
                content.text = "";
                button.onClick.AddListener(() => clickSubject.OnNext(++clickCounter));
            }

            public void SetContent(string msg)
            {
                content.text = msg;
            }
        }

        public class RXTestPresenter : IDisposable
        {
            private RXTestView view;
            private Subject<RXVoid> subject = new Subject<RXVoid>();
            public ISubject<RXVoid> ClickOver5TimesEvent => subject;

            public void Initialize()
            {
                view = Object.FindObjectOfType<RXTestView>();
                view.gameObject.SetActive(true);
                view.ClickEvent.Subscribe(OnButtonClicked);
                OnButtonClicked(0);
            }

            public void Dispose()
            {
                // Just for test.
                view.gameObject.SetActive(false);
                // In fact, we should destroy the view and dispose subjects
            }

            private void OnButtonClicked(int counter)
            {
                string content = $"button clicked times: {counter}";
                view.SetContent(content);
                if (counter >= 5)
                {
                    subject.OnNext(RXVoid.Void);
                }
            }
        }
    }
}