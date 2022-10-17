using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    /// <summary>
    /// Class for communicating with the minigame system
    /// </summary>
    public class MinigameController : MonoBehaviour
    {
        [SerializeField] FloatEventChannelSO scoreUpdateChannel;
        [SerializeField] VoidEventChannelSO startMinigameChannel;
        [SerializeField] VoidEventChannelSO endMinigameChannel;
        [SerializeField] VoidEventChannelSO cleanUpMinigameChannel;

        void Start()
        {
            startMinigameChannel.RaiseEvent();
            SetGameScore(0);
        }

        void OnDisable()
        {
            cleanUpMinigameChannel.RaiseEvent();
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
            endMinigameChannel.RaiseEvent();
        }
    }
}

