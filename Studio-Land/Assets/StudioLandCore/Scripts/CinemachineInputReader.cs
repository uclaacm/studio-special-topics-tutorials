using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace StudioLand
{
    public class CinemachineInputReader : MonoBehaviour
    {
        [SerializeField] StudioLand.InputReaderSO playerInput;
        [SerializeField] CinemachineInputProvider provider;
        void Awake()
        {
            provider.XYAxis = playerInput.GetCameraRotationActionReference;
        }
    }
}

