using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class CustomWindow : EditorWindow
{
    [MenuItem("Game/Custom Window!")]
    static void ShowWindow(UnityEditor.MenuCommand command)
    {
        GetWindow<CustomWindow>();
    }

    void CreateGUI()
    {
        rootVisualElement.Add(new Label("Hello!"));
    }
}
