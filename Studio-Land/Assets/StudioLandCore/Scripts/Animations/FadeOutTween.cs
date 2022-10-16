using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace StudioLand
{
    public class FadeOutTween : UIAnimation
    {
        [SerializeField] UIDocument document;
        [SerializeField] float fadeIntensity = 0.8f;
        [SerializeField] float fadeSpeed = 0.1f;
        public override bool IsCurrentlyAnimating => isAnimating;
        private bool isAnimating = false;

        public override void StartAnimation()
        {
            isAnimating = true;

            StyleValues oldValues;
            //oldValues.backgroundColor = new Color(0, 0, 0, fadeIntensity);

            StyleValues newValues;
            newValues.backgroundColor = new Color(0, 0, 0, 0);

            document.rootVisualElement.experimental.animation.Start(oldValues, newValues, (int)(fadeSpeed * 1000)).OnCompleted(() => isAnimating = false);
        }
    }
}

