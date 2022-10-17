using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace StudioLand
{
    public class MinigameHUD : MonoBehaviour
    {
        [SerializeField] UIDocument document;
        [SerializeField] FloatEventChannelSO scoreUpdateChannel;
        [SerializeField] VoidEventChannelSO startMinigameChannel;
        [SerializeField] VoidEventChannelSO cleanUpMinigameChannel;
        Label scoreLabel;

        void OnEnable()
        {
            scoreUpdateChannel.OnEventRaised += HandleScoreUpdate;
            startMinigameChannel.OnEventRaised += HandleMinigameStart;
            cleanUpMinigameChannel.OnEventRaised += HandleMinigameEnd;
            
            scoreLabel = document.rootVisualElement.Q<Label>("score");
        }

        void OnDisable()
        {
            scoreUpdateChannel.OnEventRaised -= HandleScoreUpdate;
            startMinigameChannel.OnEventRaised -= HandleMinigameStart;
            cleanUpMinigameChannel.OnEventRaised -= HandleMinigameEnd;
        }

        void HandleMinigameEnd()
        {
            // Hide HUD when minigame cleaned up
            document.rootVisualElement.style.display = DisplayStyle.None;
        }

        void HandleMinigameStart()
        {
            // When minigame started, show the HUD
            document.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        void HandleScoreUpdate(float newScore)
        {
            scoreLabel.text = "Score: " + newScore;
        }
    }
}

