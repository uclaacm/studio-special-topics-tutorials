using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioLand
{
    /// <summary>
    /// This class is used for Events that have a minigame argument.
    /// Example: An event to notify the instructions UI to display a new minigame
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Minigame Event Channel")]
    public class MinigameEventChannelSO : DescriptionBaseSO
    {
        public event UnityAction<MinigameSO> OnEventRaised;

        public void RaiseEvent(MinigameSO value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}   

