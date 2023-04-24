using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public enum EnemyType
{
    Static,
    Moving
}

[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    [Min(0)]
    [SerializeField] float maxHealth;

    [SerializeField] Player player;

    [SerializeField] EnemyType type;

    [SerializeField] float speed;

    // new is here because name is a (depricated) field of a base class
    [SerializeField] new string name = "";

    [SerializeField] ExampleSingletonSO exampleSingletonSO;

    // mutation is protected, so only Enemy can mutate itself
    public float Health { get; protected set; }

    void OnValidate()
    {
        name = name.ToLower();
    }

    void Reset()
    {
        // FindObjectOfType: REALLLLLLY slow, shouldn't use outside of editor
        player = FindObjectOfType<Player>();

        // Traversing asset database is also probably very slow and very annoying to do
        // But we can do it here!
        // Fun challenge: make a generic static function that does this for any type you want :)

        // typeof(ExampleSingletonSO).Name == "ExampleSingletonSO"
        // AssetDatabase.FindAssets is the same as searching in the project window ("t: TYPE" finds all assets of type TYPE)
        string[] candidateGUIDs = AssetDatabase.FindAssets($"t: {typeof(ExampleSingletonSO).Name}");
        if(candidateGUIDs.Length > 0) {
            string path = AssetDatabase.GUIDToAssetPath(candidateGUIDs[0]);
            exampleSingletonSO = AssetDatabase.LoadAssetAtPath<ExampleSingletonSO>(path);
        }
    }

    void Start()
    {
        Health = maxHealth;
    }

    void Update()
    {
        if(type == EnemyType.Moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
    }

    public void Hit(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        GUI.color = Color.red;
        Handles.Label(transform.position, $"{gameObject.name}: {Health}");
    }
}