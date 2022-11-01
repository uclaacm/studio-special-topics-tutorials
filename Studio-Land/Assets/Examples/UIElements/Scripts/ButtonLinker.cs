using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class ButtonLinker : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] string buttonTag = "";
    [SerializeField] UnityEvent OnButtonPressed;
    

    void Awake()
    {
        document.rootVisualElement.Q<Button>(buttonTag).RegisterCallback<ClickEvent>((ev) => OnButtonPressed?.Invoke());
    }
}
