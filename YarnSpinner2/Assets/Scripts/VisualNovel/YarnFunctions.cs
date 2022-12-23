using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Yarn.Unity;

/// <summary> Static Yarn Spinner functions. Functions return variables (int, string, etc.) </summary>
public static class YarnFunctions
{
    /// <summary> Returns true if the current device is a computer (not a mobile device). </summary>
    [YarnFunction("isComputer")]
    public static bool IsComputer()
    {
        return !Application.isMobilePlatform;
    }

    // Add more useful functions here!
}
