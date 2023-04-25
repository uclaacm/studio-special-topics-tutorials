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

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Vector2 minNodePosition;
    [SerializeField] Vector2 maxNodePosition;

    [SerializeField] LayerMask wallLayerMask;

    [SerializeField] List<Vector2> traversableNodes;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Pathfinding)), CanEditMultipleObjects]
public class PathfindingEditor : Editor
{
    void OnSceneGUI()
    {
        var targetSO = new SerializedObject(target);

        var minSP = targetSO.FindProperty("minNodePosition");
        var maxSP = targetSO.FindProperty("maxNodePosition");

        ///TODO: Create position handles for above properties
        ///TODO: Apply changes to above properties

        // Note: Array traversal with properties is tricky
        var traversableNodes = targetSO.FindProperty("traversableNodes").Copy();

        // The first element is the array type
        // true to "enter" the array object
        traversableNodes.Next(true);

        // The second element is the array size
        traversableNodes.Next(true);

        int size = traversableNodes.intValue;

        // The 3rd element is the array itself
        // True to actually enter the array
        traversableNodes.Next(true);

        for(int i = 0; i < size; ++i)
        {
            Vector2 pos = traversableNodes.vector2Value;

            ///TODO: Draw some sort of indicator at the position
            ///(NOTE: You cannot use Gizmos in editor scripts
            ///You can use any handles function though, a useful one is
            ///Handles.DrawSolidRectangleWithOutline)

            // we dont want to go next past end of array
            if (i != size - 1)
            {
                // false because we don't want to enter the array elements
                traversableNodes.Next(false);
            }
        }
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();
        
        ///TODO: Create default inspector and add button at bottom to recalculate traversable nodes

        return inspector;
    }

    void RecalculateTraversableNodes()
    {
        LayerMask wallLayerMask = (LayerMask)serializedObject.FindProperty("wallLayerMask").intValue;
        Vector2 min = serializedObject.FindProperty("minNodePosition").vector2Value;
        Vector2 max = serializedObject.FindProperty("maxNodePosition").vector2Value;

        List<Vector2> traversablePoints = new List<Vector2>();

        ///TODO: See if the points between min and max are traversable
        ///Physics2D.OverlapPoint(position, wallLayerMask) is how to check
        ///if the point has a wall on it
        
        var so = serializedObject.FindProperty("traversableNodes");
        so.ClearArray();
        for(int i = 0; i < traversablePoints.Count; ++i)
        {
            so.InsertArrayElementAtIndex(i);
            so.GetArrayElementAtIndex(i).vector2Value = traversablePoints[i];
        }
        serializedObject.ApplyModifiedProperties();
        //Update to redraw inspector
        serializedObject.Update();
    }
}
#endif