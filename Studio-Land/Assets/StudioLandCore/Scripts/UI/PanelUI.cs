using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace StudioLand
{
    public abstract class PanelUI : MonoBehaviour
    {
        [SerializeField] UIAnimation entranceAnimation;
        [SerializeField] UIAnimation closingAnimation;
        [SerializeField] UIDocument document;
        [SerializeField] UIFocuser focuser;

        [SerializeField] UnityEvent OnAnimateIn = new UnityEvent();
        [SerializeField] UnityEvent OnAnimateOut = new UnityEvent();
        [SerializeField] UnityEvent OnReset = new UnityEvent();
        protected VisualElement root;

        public abstract bool CanDeselect {get;}

        public bool isAnimating => entranceAnimation.IsCurrentlyAnimating || closingAnimation.IsCurrentlyAnimating;


        protected virtual void Awake()
        {
            Reset();
        }

        public void AnimateIn()
        {
            entranceAnimation?.StartAnimation();
            OnAnimateIn?.Invoke();
        }
        public void AnimateOut()
        {
            closingAnimation?.StartAnimation();
            OnAnimateOut?.Invoke();
        }

        public void Reset()
        {
            root = document.rootVisualElement;
            root.style.opacity = 0;
            root.style.display = DisplayStyle.None;
            OnReset?.Invoke();
        }
    }
}

