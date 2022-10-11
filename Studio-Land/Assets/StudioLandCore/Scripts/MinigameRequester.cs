using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace StudioLand
{
    /// <summary>
    /// Simple component for notifying the game that a player wants to play a minigame
    /// </summary>
    public class MinigameRequester : MonoBehaviour
    {
        // Note: Even though this behaviour doesn't really NEED to be a monobehaviour (since it's mostly functional),
        // having it exist in the scene as a component is easier to understand than a ScriptableObject asset, which
        // needs to be managed outside of the scene.
        [SerializeField] MinigameSO minigame;
        [SerializeField] MinigameEventChannelSO minigameEventChannelSO;
        [SerializeField] UnityEvent<MinigameSO> OnValidate = new UnityEvent<MinigameSO>();
        bool isValid
        {
            get => minigame != null && minigame.scene.editorAsset != null;
        }

        void Start()
        {
            if(isValid)
                OnValidate?.Invoke(minigame);
        }

        public void PlayMinigame()
        {
            if(isValid)
                minigameEventChannelSO.RaiseEvent(minigame);

            // TODO: Send an error status to some error module to display on the UI if not valid

        }

    }
}

