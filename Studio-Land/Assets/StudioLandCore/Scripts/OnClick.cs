using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace StudioLand
{
    public class OnClick : MonoBehaviour
    {
        [SerializeField] UIDocument document;
        [SerializeField] UnityEvent onClick;

        void OnEnable()
        {
            document.rootVisualElement.RegisterCallback<ClickEvent>(ev => onClick?.Invoke());
        }
    }
}

