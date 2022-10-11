using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace StudioLand
{
    public class AppearFromBelowTween : UIAnimation
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
            // Enable panel
            root.style.display = DisplayStyle.Flex;

            StyleValues oldValues;
            oldValues.opacity = 0;
            oldValues.top = 200;

            StyleValues newValues;
            newValues.opacity = 1;
            newValues.top = 0;

            root.experimental.animation.Start(oldValues, newValues, (int)(duration * 1000)).onAnimationCompleted += () => currentlyAnimating = false;
            
        }

        public override bool IsCurrentlyAnimating => currentlyAnimating;
    }
}

