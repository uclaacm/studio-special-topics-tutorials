using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;   // Used for UnityEvents;

public static class FinalSettings
{

    public static UnityEvent<float> onTextDelayChanged = new UnityEvent<float>();

    public static float TEXT_DELAY
    {
    	get
    	{
    		// Return value of TEXT_DELAY from PlayerPrefs, defaulting to 0.05f if no value found
    		return PlayerPrefs.GetFloat("TEXT_DELAY", 0.05f);
    	}
    	set
    	{
    		// Store new value of TEXT_DELAY into PlayerPrefs
    		PlayerPrefs.SetFloat("TEXT_DELAY", value);
            // Notify observers of change in text delay
            onTextDelayChanged.Invoke(value);
    	}
    }
}
