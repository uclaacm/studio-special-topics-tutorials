using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;	// Used for Slider

public class DelegateTextDelaySlider : MonoBehaviour
{

	private Slider slider;

	// Get slider component at start of scene
	void Awake()
	{
		slider = GetComponent<Slider>();
	}

	// Set initial value of slider and add listener to react to slider
    void Start()
    {
        slider.value = DelegateSettings.TEXT_DELAY;
        slider.onValueChanged.AddListener(ChangeTextDelay);	// Don't use () after function name in AddListener
    }

    // Update value of TEXT_DELAY in Settings
    private void ChangeTextDelay(float delay)
    {
    	DelegateSettings.TEXT_DELAY = delay;
    }
}
