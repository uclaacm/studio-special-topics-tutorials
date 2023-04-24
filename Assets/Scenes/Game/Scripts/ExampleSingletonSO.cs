using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExampleSingleton", menuName = "Example Singleton")]
public class ExampleSingletonSO : ScriptableObject
{
    [SerializeField] public int Data = 0;

    public void Reset()
    {
        Data = new System.Random().Next();
    }
}
