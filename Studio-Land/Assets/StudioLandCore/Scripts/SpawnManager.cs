using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject playerPrefab;
        [SerializeField] TransformAnchor playerTransformAnchor;
        [SerializeField] Transform spawnLocation;

        [Header("Listens on")]
        [SerializeField] VoidEventChannelSO sceneReadyEventChannel;

        void OnEnable()
        {
            sceneReadyEventChannel.OnEventRaised += HandleSceneReady;
        }

        void OnDisable()
        {
            sceneReadyEventChannel.OnEventRaised -= HandleSceneReady;
        }

        void HandleSceneReady()
        {
            var player = Instantiate(playerPrefab, spawnLocation);
            playerTransformAnchor.Provide(player.transform);
        }
    }
}

