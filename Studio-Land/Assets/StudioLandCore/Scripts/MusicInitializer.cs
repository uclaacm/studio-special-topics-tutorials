using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class MusicInitializer : MonoBehaviour
    {
        [SerializeField] AudioCueSO music;
        [SerializeField] AudioConfigurationSO musicConfigurationSO;

        [Header("Listens on")]
        [SerializeField] VoidEventChannelSO sceneReadyChannelSO;

        [Header("Broadcasts on")]
        [SerializeField] AudioCueEventChannelSO musicEventChannelSO;



        void OnEnable()
        {
            sceneReadyChannelSO.OnEventRaised += HandleSceneReadied;
        }

        void OnDisable()
        {
            sceneReadyChannelSO.OnEventRaised -= HandleSceneReadied;
        }

        void HandleSceneReadied()
        {
            musicEventChannelSO.RaisePlayEvent(music, musicConfigurationSO);
        }
    }
}

