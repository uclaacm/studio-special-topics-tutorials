using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace StudioLand
{
    [CreateAssetMenu( fileName = "New MinigameSO", menuName = "ScriptableObjects/Minigame")]
    public class MinigameSO : ScriptableObject
    {
        public string title = default;
        public int highestScore = 0;
        public AssetReference scene = default;

        [TextArea(3, 10)]
        public string instructions = default;

        public Sprite picture = default;
    }
}

