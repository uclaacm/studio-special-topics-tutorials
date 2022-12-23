using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SelectOnAppear : MonoBehaviour
{
    public bool onlyWithGamepad = false;

    void OnEnable()
    {
        if (!onlyWithGamepad || Gamepad.current != null) EventSystem.current?.SetSelectedGameObject(gameObject);
    }
}
