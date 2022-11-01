using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for auto-enabling UI input to showcase UI Element capabilities
/// </summary>
public class UIEnabler : MonoBehaviour
{
    [SerializeField] StudioLand.InputReaderSO input;
    void Update()
    {
        // An incredibly dangerous and inefficient way of allowing us to play with the UI Elements system.
        // Don't do this in a real project.
        // Really.
        // I'm just doing this because it's easy for this tutorial.
        input.EnableUIInput();
    }


}
