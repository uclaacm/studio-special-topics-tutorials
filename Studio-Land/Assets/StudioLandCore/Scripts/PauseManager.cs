using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] InputReaderSO playerInput;

        [Header("Listens to")]
        [SerializeField] VoidEventChannelSO pauseEvent;

        void Awake()
        {
            playerInput.QuitEvent += HandlePlayerQuit;
        }

        void HandlePlayerQuit()
        {
            pauseEvent.RaiseEvent();
        }
    }
}

