using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;	// Used for StringBuilder
using TMPro;		// Used for TextMeshPro

public class Typewriter : MonoBehaviour
{

	[SerializeField] private string exampleLine;	// Line of dialogue to display
	[SerializeField] private float delay = 0.05f;	// How long to wait between characters
	private TextMeshProUGUI textMesh;				// TextMeshPro component

	private int displayed = 0;						// Number of characters displayed
	private StringBuilder sb = new StringBuilder();	// StringBuilder representing displayed text
	private float time;								// Time since last update of line

	void Awake()
	{
		textMesh = GetComponent<TextMeshProUGUI>();	// Get TextMeshPro component
	}

	void Start()
	{
		textMesh.text = sb.ToString();				// Set starting text to be empty
		time = Time.time;							// Start keeping track of time
	}

	void Update()
	{
		if (displayed < exampleLine.Length)			// Only update line if we haven't finished
		{
			if (Time.time - time > delay)			// Only update line if text delay reached
			{
				sb.Append(exampleLine[displayed]);	// Add next character to StringBuilder
				displayed++;						// Update number of characters
				textMesh.text = sb.ToString();		// Update TextMeshPro component
				time = Time.time;					// Update time since last character
			}
		}
	}
}
