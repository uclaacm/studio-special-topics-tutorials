using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class MinigameController : MonoBehaviour
    {
        [SerializeField] FloatEventChannelSO scoreUpdateChannel;
        [SerializeField] VoidEventChannelSO startGameChannel;
        [SerializeField] VoidEventChannelSO endGameChannel;

        void Awake()
        {
            startGameChannel.RaiseEvent();
        }

        /// <summary>
        /// Update current score for the minigame
        /// </summary>
        public void SetGameScore(float newScore)
        {
            scoreUpdateChannel.RaiseEvent(newScore);
        }

        /// <summary>
        /// End the minigame and save the current score as the final score
        /// </summary>
        public void EndGame()
        {
            endGameChannel.RaiseEvent();
        }
    }
}

