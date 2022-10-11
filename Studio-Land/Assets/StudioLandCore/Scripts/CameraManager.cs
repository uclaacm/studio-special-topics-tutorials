using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace StudioLand
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] TransformAnchor playerTransformAnchor;
        [SerializeField] CinemachineFreeLook freeLookCamera;

        void OnEnable()
        {
            playerTransformAnchor.OnAnchorProvided += HandlePlayerTransformInitialized;
        }

        void OnDisable()
        {
            playerTransformAnchor.OnAnchorProvided -= HandlePlayerTransformInitialized;
        }

        void HandlePlayerTransformInitialized()
        {
            freeLookCamera.Follow = playerTransformAnchor.Value;
            freeLookCamera.LookAt = playerTransformAnchor.Value;
        }
    }
}

