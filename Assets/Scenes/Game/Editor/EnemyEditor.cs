//#define ENEMY_EDITOR_VARIANT_DEFAULT
//#define ENEMY_EDITOR_VARIANT_CUSTOM
//#define ENEMY_EDITOR_VARIANT_VISUAL_TREE
//#define ENEMY_EDITOR_VARIANT_DEFAULT_INSPECTOR_FOLDOUT
#define ENEMY_EDITOR_VARIANT_DEFAULT_MODIFIED
//#define ENEMY_EDITOR_VARIANT_BUTTONS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.ComponentModel.Design;

#if ENEMY_EDITOR_VARIANT_DEFAULT
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        return base.CreateInspectorGUI();
    }
}
#elif ENEMY_EDITOR_VARIANT_CUSTOM
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        inspector.Add(new Label("Hello from Editor!"));

        inspector.Add(new PropertyField(
            serializedObject.FindProperty("maxHealth")
        ));

        return inspector;
    }
}
#elif ENEMY_EDITOR_VARIANT_VISUAL_TREE
public class EnemyEditor : Editor
{
    [SerializeField] VisualTreeAsset visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = visualTree.CloneTree();

        return inspector;
    }
}
#elif ENEMY_EDITOR_VARIANT_DEFAULT_INSPECTOR_FOLDOUT
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        inspector.Add(new Label("Hello from Editor!"));

        inspector.Add(new PropertyField(
            serializedObject.FindProperty("maxHealth")
        ));

        var defaultFoldout = new Foldout();
        defaultFoldout.text = "Default Inspector";
        defaultFoldout.value = false;
        InspectorElement.FillDefaultInspector(defaultFoldout, serializedObject, this);

        inspector.Add(defaultFoldout);

        return inspector;
    }
}
#elif ENEMY_EDITOR_VARIANT_DEFAULT_MODIFIED
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    List<PropertyField> movingEnemyPropertyFields = new List<PropertyField>();

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

        // iterate through children
        foreach (var child in inspector.Children())
        {
            if(child is PropertyField && (child as PropertyField).bindingPath == "type")
            {
                (child as PropertyField).RegisterValueChangeCallback(
                    evt => UpdateMovingEnemyPropertyFieldsVisibility(evt.changedProperty)
                );
            }

            // add property fields bound to "speed" to the moving enemy property list
            if (child is PropertyField && (child as PropertyField).bindingPath == "speed")
            {
                movingEnemyPropertyFields.Add(child as PropertyField);
            }
        }

        UpdateMovingEnemyPropertyFieldsVisibility(serializedObject.FindProperty("type"));

        return inspector;
    }

    void UpdateMovingEnemyPropertyFieldsVisibility(SerializedProperty enemyTypeProperty)
    {
        // get enemy type
        var enemyType = (EnemyType)enemyTypeProperty.enumValueIndex;

        // make all movingEnemyPropertyFields visible or invisible dependent on the enemy type
        foreach (var propertyField in movingEnemyPropertyFields)
        {
            if (enemyType == EnemyType.Static)
            {
                propertyField.style.display = DisplayStyle.None;
            }
            else
            {
                propertyField.style.display = DisplayStyle.Flex;
            }
        }
    }
}
#elif ENEMY_EDITOR_VARIANT_BUTTONS
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

        var doubleHealthButton = new Button(DoubleHealth);
        doubleHealthButton.text = "Double Health";
        inspector.Add(doubleHealthButton);

        return inspector;
    }

    void DoubleHealth()
    {
        serializedObject.FindProperty("maxHealth").floatValue = 2 * serializedObject.FindProperty("maxHealth").floatValue;
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public static class EnemyEditorUtil
{
    [MenuItem("Game/What is this . . . ?")]
    static void TellMeImPretty(UnityEditor.MenuCommand menuCommand)
    {
        EditorUtility.DisplayDialog("UwU", "You're so pretty :3", "Thank you!");
    }

    [MenuItem("CONTEXT/Enemy/WHAT'S MY NAME")]
    static void NameFinder(UnityEditor.MenuCommand menuCommand)
    {
        var enemySO = new SerializedObject(menuCommand.context as Enemy);
        string name = enemySO.FindProperty("name").stringValue;
        string message;
        if(name == null || name.Length == 0)
        {
            message = "YOUR NAME IS empty 3:";
        }
        else
        {
            message = $"YOUR NAME IS {name}";
        }
        EditorUtility.DisplayDialog("NAME", message, "Wow :o");
    }

    [MenuItem("GameObject/Game/Enemy")]
    static void EnemyGameObjectMenu(UnityEditor.MenuCommand menuCommand)
    {
        // Note: for this to work, "Enemy.prefab" must be in a folder called "Resources"
        GameObject go = Object.Instantiate(Resources.Load<GameObject>("Enemy"));
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}