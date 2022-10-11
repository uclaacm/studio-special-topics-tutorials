using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace StudioLand
{
    public class DisappearBelowTween : UIAnimation
    {
        [SerializeField] UIDocument document;
        [SerializeField] float duration = 1;
        [SerializeField] bool playOnAwake = false;
        VisualElement root;

        bool currentlyAnimating = false;

        void Awake()
        {
            root = document.rootVisualElement;

            if(playOnAwake)
                StartAnimation();
        }

        public override void StartAnimation()
        {
            // TODO: Animate the position of the document by adding it to the old and new style values struct
            StyleValues oldValues;
            oldValues.opacity = 1;
            oldValues.top = 0;

            StyleValues newValues;
            newValues.opacity = 0;
            newValues.top = 200;

            currentlyAnimating = true;

            void OnAnimationFinish()
            {
                currentlyAnimating = false;
                root.style.display = DisplayStyle.None;
            }
            root.experimental.animation.Start(oldValues, newValues, (int)(duration * 1000)).onAnimationCompleted += OnAnimationFinish;
            
        }

        public override bool IsCurrentlyAnimating => currentlyAnimating;
    }
}

