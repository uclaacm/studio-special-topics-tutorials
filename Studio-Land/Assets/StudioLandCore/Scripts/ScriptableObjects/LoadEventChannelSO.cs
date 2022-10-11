using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;

namespace StudioLand
{
    [CreateAssetMenu(menuName = "Events/Load Event Channel")]
    public class LoadEventChannelSO : ScriptableObject
    {
        public event UnityAction<AssetReference> OnEventRaised;

        public void RaiseEvent(AssetReference value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}

