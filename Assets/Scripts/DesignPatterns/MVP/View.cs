using System;
using System.Collections.Generic;
using System.Linq;
using DesignPatterns.Extension;
using DesignPatterns.MVP.ViewLoader;
using UnityEngine;

namespace DesignPatterns.MVP
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        private static readonly IViewLoader viewLoader = new EditorViewLoader();


        public static void LoadView<T>(Presenter presenter) where T : View
        {
            viewLoader.LoadView<T>(view => { presenter.OnViewLoaded(view); });
        }

        protected bool disposed;
        protected List<IDisposable> disposables = new List<IDisposable>();


        public virtual void Appear(Action action)
        {
            gameObject.SetActive(true);
            action();
        }

        public virtual void Disappear(Action action)
        {
            gameObject.SetActive(false);
            action();
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            for (var i = 0; i < disposables.Count; i++)
            {
                disposables[i].Dispose();
            }

            disposables.Clear();
            UnityEngine.GameObject.Destroy(this);
        }
    }

    public abstract class AnimationView : View
    {
        public readonly string AnimationNameAppear = "AnimationAppear";
        public readonly string AnimationNameDisappear = "AnimationDisappear";

        public override void Appear(Action action)
        {
            gameObject.SetActive(true);
            AnimationEventAttach.Play(GetComponent<Animator>(), AnimationNameAppear, action);
        }


        public override void Disappear(Action action)
        {
            AnimationEventAttach.Play(GetComponent<Animator>(), AnimationNameDisappear, () =>
            {
                gameObject.SetActive(false);
                action();
            });
        }
    }

    // todo: Pool it ?
    public class AnimationEventAttach : MonoBehaviour
    {
        public static void Play(Animator animator, string animationName, Action callback)
        {
            AnimationEventAttach eventAttach = animator.gameObject.AddComponent<AnimationEventAttach>();
            eventAttach.Initialize(animator, animationName, callback);
        }

        [Header("Those value make from runtime, do not edit it")] [SerializeField] private string cachedAnimationName;
        [SerializeField] private bool isEventAdded;
        [SerializeField] private Animator cachedAnimator;
        private Action cachedCallback;

        private void Initialize(Animator animator, string animationName, Action callback)
        {
            cachedAnimator = animator;
            cachedAnimationName = animationName;
            cachedCallback = callback;

            animator.Play(cachedAnimationName, -1, 0);
            // Add animation event on next frame
            isEventAdded = false;
        }

        private void LateUpdate()
        {
            if (isEventAdded)
                return;
            isEventAdded = true;

            // todo There need to check
            // Is there need create 2 animators for 2 animations ?
            AnimationClip clip = cachedAnimator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == cachedAnimationName);
            Debug.Assert(clip, "clip");
            clip.AddEvent(new AnimationEvent()
            {
                time = clip.length,
                functionName = nameof(OnAnimationFinished),
                intParameter = GetHashCode()
            });
        }

        private void OnAnimationFinished(int param)
        {
            if (param != GetHashCode())
                return;
            if (cachedCallback != null)
            {
                cachedCallback.Invoke();
            }

            Destroy(this);
            // Debug.Log($"Destroy {cachedAnimationName}");
        }
    }
}