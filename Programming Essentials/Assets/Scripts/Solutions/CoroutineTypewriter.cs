using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;	// Used for StringBuilder
using TMPro;		// Used for TextMeshPro

public class CoroutineTypewriter : MonoBehaviour
{

	[SerializeField] private float delay = 0.05f;      // How long to wait between characters
	private TextMeshProUGUI textMesh;                  // TextMeshPro component
	private Coroutine typewriter;                      // Ongoing coroutine, if any

	void Awake()
	{
		textMesh = GetComponent<TextMeshProUGUI>();    // Get TextMeshPro component
	}

	// Wrapper function for TypewriterCoroutine that starts/stops the coroutine
	public void StartTypewriter(string line)
	{
		if (line != null)
		{
			if (typewriter != null)
			{
				StopCoroutine(typewriter);
			}
			typewriter = StartCoroutine(TypewriterCoroutine(line));
		}
	}

	// Displays text one character at a time, with a delay in between each character
	private IEnumerator TypewriterCoroutine(string line)
	{
		StringBuilder sb = new StringBuilder();
		textMesh.text = sb.ToString();

		foreach (char c in line)
		{
			sb.Append(c);
			textMesh.text= sb.ToString();
			if (delay > 0)
				yield return new WaitForSeconds(delay);
		}
	}
}
