using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioLand
{
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] VoidEventChannelSO voidEventChannel;
        [SerializeField] UnityEvent callback = new UnityEvent();
        void Awake()
        {
            voidEventChannel.OnEventRaised += () => callback?.Invoke();
        }
    }
}

