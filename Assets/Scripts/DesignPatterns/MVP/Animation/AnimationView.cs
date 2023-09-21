using System;
using UnityEngine;

namespace DesignPatterns.MVP.Animation
{
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
}