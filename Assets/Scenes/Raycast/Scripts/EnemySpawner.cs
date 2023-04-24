using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Vector3 minPosition;
    [SerializeField] Vector3 maxPosition;

    public void SpawnRandom()
    {
        Instantiate(
            enemyPrefab, 
            transform.position 
            + new Vector3(
                (Random.Range(0f, 1f) * (maxPosition - minPosition) + minPosition).x,
                0,
                + (Random.Range(0f, 1f) * (maxPosition - minPosition) + minPosition).z
            ),
            transform.rotation
        );
    }

    public void DeleteAllEnemies()
    {
        RaycastEnemy[] enemies = FindObjectsOfType<RaycastEnemy>();
        foreach(var enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                DestroyImmediate(enemy.gameObject);
            }
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    void OnSceneGUI()
    {
        EnemySpawner enemySpawner = target as EnemySpawner;

        // Add and delete enemy GUI
        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(Screen.width / 2, Screen.height / 2, 300, 300));

        if (GUILayout.Button("Add Random Enemy", GUILayout.Width(100)))
        {
            enemySpawner.SpawnRandom();
        }

        if (GUILayout.Button("Delete All Enemies", GUILayout.Width(100)))
        {
            enemySpawner.DeleteAllEnemies();
        }

        GUILayout.EndArea();

        Handles.EndGUI();

        // min and max position handles
        EditorGUI.BeginChangeCheck();
        Vector3 newMin = Handles.PositionHandle(
            serializedObject.FindProperty("minPosition").vector3Value,
            Quaternion.identity
        );
        Vector3 newMax = Handles.PositionHandle(
            serializedObject.FindProperty("maxPosition").vector3Value,
            Quaternion.Euler(0, 180, 0)
        );
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.FindProperty("minPosition").vector3Value = newMin;
            serializedObject.FindProperty("maxPosition").vector3Value = newMax;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif