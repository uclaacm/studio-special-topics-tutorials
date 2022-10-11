using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioLand
{
    public class FloatEventListener : MonoBehaviour
    {
        [SerializeField] FloatEventChannelSO floatEventChannel;
        [SerializeField] UnityEvent<float> callback = new UnityEvent<float>();
        void Awake()
        {
            floatEventChannel.OnEventRaised += (val) => callback?.Invoke(val);
        }
    }
}

