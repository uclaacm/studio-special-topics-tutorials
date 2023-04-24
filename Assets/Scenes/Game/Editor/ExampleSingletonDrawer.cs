using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ExampleSingletonSO))]
public class ExampleSingletonDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var target = property.objectReferenceValue as ExampleSingletonSO;
        var targetSO = new SerializedObject(target);

        Foldout drawer = new Foldout();
        drawer.text = property.displayName;

        drawer.Add(new PropertyField(property, ""));
        var dataField = new IntegerField("Data");
        drawer.Add(dataField);
        dataField.BindProperty(targetSO.FindProperty("Data"));

        var resetButton = new Button(() => { target.Reset(); });
        resetButton.text = "Reset";
        drawer.Add(resetButton);

        return drawer;
    }
}
