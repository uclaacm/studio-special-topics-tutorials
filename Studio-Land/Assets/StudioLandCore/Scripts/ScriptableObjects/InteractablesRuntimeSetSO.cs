using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    [CreateAssetMenu(fileName = "New InteractablesRuntimeSet", menuName = "ScriptableObjects/Interactables Runtime Set")]
    public class InteractablesRuntimeSetSO : ScriptableObject
    {
        public List<Interactable> runtimeSet = new List<Interactable>();
    }
}

