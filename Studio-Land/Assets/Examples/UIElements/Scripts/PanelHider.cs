using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelHider : MonoBehaviour
{
    [SerializeField] UIDocument document;
    [SerializeField] bool hideOnAwake = true;

    void Awake()
    {
        SetHideState(hideOnAwake);
    }

    public void SetHideState(bool shouldHide)
    {
        document.rootVisualElement.style.display = shouldHide? DisplayStyle.None : DisplayStyle.Flex;
    }
}
