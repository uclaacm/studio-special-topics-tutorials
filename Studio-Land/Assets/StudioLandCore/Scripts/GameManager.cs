using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class GameManager : MonoBehaviour
    {
        enum GameState
        {
            Gameplay,
            Interaction
        }
        GameState currentState = GameState.Gameplay;

        [SerializeField] InputReaderSO inputReader;

        [Header("Listens on")]
        [SerializeField] VoidEventChannelSO sceneReadyChannelSO;
        [SerializeField] VoidEventChannelSO interactionWindowOpenedChannelSO;
        [SerializeField] VoidEventChannelSO interactionWindowClosedChannelSO;
        [SerializeField] VoidEventChannelSO pauseWindowOpenedChannelSO;
        [SerializeField] VoidEventChannelSO pauseWindowClosedChannelSO;

        void OnEnable()
        {
            sceneReadyChannelSO.OnEventRaised += HandleSceneReady;

            interactionWindowOpenedChannelSO.OnEventRaised += HandleInteractionStart;
            interactionWindowClosedChannelSO.OnEventRaised += HandleInteractionEnd;
            pauseWindowOpenedChannelSO.OnEventRaised += HandlePauseStart;
            pauseWindowClosedChannelSO.OnEventRaised += HandlePauseEnd;

        }

        void OnDisable()
        {
            sceneReadyChannelSO.OnEventRaised -= HandleSceneReady;

            interactionWindowOpenedChannelSO.OnEventRaised -= HandleInteractionStart;
            interactionWindowClosedChannelSO.OnEventRaised -= HandleInteractionEnd;
            pauseWindowOpenedChannelSO.OnEventRaised -= HandlePauseStart;
            pauseWindowClosedChannelSO.OnEventRaised -= HandlePauseEnd;
        }

        void HandleSceneReady()
        {
            // Once everything in the scene is ready, we enter the gameplay state immediately
            // NOTE: Later on we may want a dedicated state tracking variable
            inputReader.EnableGameplayInput();
            currentState = GameState.Gameplay;
        }

        void HandleInteractionStart()
        {
            inputReader.EnableUIInput();
            currentState = GameState.Interaction;
        }

        void HandleInteractionEnd()
        {
            inputReader.EnableGameplayInput();
            currentState = GameState.Gameplay;
        }

        void HandlePauseStart()
        {
            inputReader.EnableUIInput();
        }

        void HandlePauseEnd()
        {
            if(currentState == GameState.Gameplay)
                inputReader.EnableGameplayInput();
        }
    }
}

