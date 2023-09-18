using System;
using DesignPatterns.MVP.ViewLoader;
using UnityEngine;
using UnityEngine.Serialization;

namespace DesignPatterns.MVP
{
    public abstract class View : MonoBehaviour, IDisposable
    {
        private static readonly IViewLoader viewLoader = new EditorViewLoader();

        public static void LoadView<T>(Presenter presenter) where T : View
        {
            viewLoader.LoadView<T>(view => { presenter.OnViewLoaded(view); });
        }


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

    public class AnimationEventAttach : MonoBehaviour
    {
        public static void Play(Animator animator, string animationName, Action callback)
        {
            AnimationEventAttach eventAttach = animator.gameObject.AddComponent<AnimationEventAttach>();
            eventAttach.Initialize(animator, animationName, callback);
        }

        [Header("Those value make from runtime, do not edit it")]
        [SerializeField] private string cachedAnimationName;
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
            AnimationClip clip = cachedAnimator.runtimeAnimatorController.animationClips[0];
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
            Debug.Log($"Destroy {cachedAnimationName}");
        }
    }
}