using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Tilemaps;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

public class PathfindingSolution : MonoBehaviour
{
    [SerializeField] Vector2 minNodePosition;
    [SerializeField] Vector2 maxNodePosition;

    [SerializeField] LayerMask wallLayerMask;

    [SerializeField] List<Vector2> traversableNodes;
}

#if UNITY_EDITOR
[CustomEditor(typeof(PathfindingSolution)), CanEditMultipleObjects]
public class PathfindingSolutionEditor : Editor
{
    void OnSceneGUI()
    {
        var targetSO = new SerializedObject(target);

        var minSP = targetSO.FindProperty("minNodePosition");
        var maxSP = targetSO.FindProperty("maxNodePosition");

        minSP.vector2Value = (Vector2)Handles.PositionHandle((Vector3)minSP.vector2Value, Quaternion.identity);
        maxSP.vector2Value = (Vector2)Handles.PositionHandle((Vector3)maxSP.vector2Value, Quaternion.Euler(0, 0, 180));
        targetSO.ApplyModifiedProperties();

        var traversableNodes = targetSO.FindProperty("traversableNodes").Copy();

        traversableNodes.Next(true);
        traversableNodes.Next(true);


        int size = traversableNodes.intValue;

        traversableNodes.Next(true); // enter array

        for (int i = 0; i < size; ++i)
        {
            Vector2 pos = traversableNodes.vector2Value;
            Handles.DrawSolidRectangleWithOutline(
                new Rect(pos - Vector2.one / 2f + Vector2.one * 0.1f, Vector2.one - 2 * Vector2.one * 0.1f),
                new Color(0, 1, 0, 0.1f),
                new Color(0, 0, 0, 0.6f)
            );
            if (i != size - 1)
            {
                traversableNodes.Next(false);
            }
        }
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

        var button = new Button(RecalculateTraversableNodes);
        button.text = "Recalculate";
        inspector.Add(button);

        return inspector;
    }

    void RecalculateTraversableNodes()
    {
        var wallLayerMask = (LayerMask)serializedObject.FindProperty("wallLayerMask").intValue;
        var min = serializedObject.FindProperty("minNodePosition").vector2Value;
        var max = serializedObject.FindProperty("maxNodePosition").vector2Value;

        List<Vector2> traversablePoints = new List<Vector2>();

        for (int x = Mathf.CeilToInt(min.x); x < max.x; ++x)
        {
            for (int y = Mathf.CeilToInt(min.y); y < max.y; ++y)
            {
                var v = new Vector2(x, y);
                if (!Physics2D.OverlapPoint(v, wallLayerMask))
                {
                    traversablePoints.Add(v);
                }
            }
        }


        var so = serializedObject.FindProperty("traversableNodes");
        so.ClearArray();
        for (int i = 0; i < traversablePoints.Count; ++i)
        {
            so.InsertArrayElementAtIndex(i);
            so.GetArrayElementAtIndex(i).vector2Value = traversablePoints[i];
        }
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif