using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioLand
{
    public class MinigameEventListener : MonoBehaviour
    {
        [SerializeField] MinigameEventChannelSO minigameEventChannel;
        [SerializeField] UnityEvent<MinigameSO> callback = new UnityEvent<MinigameSO>();
        void Awake()
        {
            minigameEventChannel.OnEventRaised += (minigame) => callback?.Invoke(minigame);
        }
    }
}

