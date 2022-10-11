using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace StudioLand
{
    public class UIFocuser : MonoBehaviour
    {
        [SerializeField] UIDocument deselectBackground;
        [SerializeField] InputReaderSO playerInput;
        [SerializeField] UnityEvent OnFocus = new UnityEvent();
        [SerializeField] UnityEvent OnDefocus = new UnityEvent();
        PanelUI currentFocus;
        PanelUI previousFocus;


        void Awake()
        {
            deselectBackground.rootVisualElement.style.display = DisplayStyle.None; // Make sure exit panel is not clickable and thus does not block anything from the start
        }
        public void RequestFocus(PanelUI panel)
        {
            //Defocus();

            if(currentFocus == null && (previousFocus == null || !previousFocus.isAnimating))
                Focus(panel);
        }

        void Focus(PanelUI panel)
        {
            deselectBackground.rootVisualElement.style.display = panel.CanDeselect? DisplayStyle.Flex : DisplayStyle.None;
            //playerInput.EnableUIInput();
            panel.AnimateIn();
            currentFocus = panel;

            OnFocus?.Invoke();
        }

        public void Defocus()
        {
            currentFocus?.AnimateOut();
            previousFocus = currentFocus;
            currentFocus = null;
            deselectBackground.rootVisualElement.style.display = DisplayStyle.None;

            OnDefocus?.Invoke();
        }

        /// <summary> Clean up the UI </summary>
        public void Reset()
        {
            currentFocus?.Reset();
            currentFocus = null;
            deselectBackground.rootVisualElement.style.display = DisplayStyle.None;
        }


    }
}

