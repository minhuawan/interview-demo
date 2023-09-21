using System;
using System.Linq;
using UnityEngine;

// todo: Pool it ?
namespace DesignPatterns.MVP.Animation
{
    public class AnimationEventAttach : MonoBehaviour
    {
        public static void Play(Animator animator, string animationName, Action callback)
        {
            Debug.Log($"{nameof(AnimationEventAttach.Play)} {animationName}");
            AnimationEventAttach eventAttach = animator.gameObject.AddComponent<AnimationEventAttach>();
            eventAttach.Initialize(animator, animationName, callback);
        }

        [Header("Those value make from runtime, do not edit it")] [SerializeField]
        private string cachedAnimationName;

        [SerializeField] private bool isEventAdded;
        [SerializeField] private Animator cachedAnimator;
        private Action cachedCallback;

        private void Initialize(Animator animator, string animationName, Action callback)
        {
            cachedAnimator = animator;
            cachedAnimationName = animationName;
            cachedCallback = callback;

            if (!animator.enabled)
                animator.enabled = true;
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