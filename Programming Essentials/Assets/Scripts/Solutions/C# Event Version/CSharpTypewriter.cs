using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;	// Used for StringBuilder
using TMPro;		// Used for TextMeshPro

[RequireComponent(typeof(TextMeshProUGUI))]

public class CSharpTypewriter : MonoBehaviour
{

	private TextMeshProUGUI textMesh;                  // TextMeshPro component
	private Coroutine typewriter;                      // Ongoing coroutine, if any
    private float delay;                               // Amount of time between characters

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();    // Get TextMeshPro component
    }

    void Start()
    {
        delay = CSharpSettings.TEXT_DELAY;                 // Initialize value of delay
        CSharpSettings.onTextDelayChanged += UpdateDelay;  // Subscribe for changes to text delay
    }

    void OnDestroy()
    {
        CSharpSettings.onTextDelayChanged -= UpdateDelay;  // Must unsubscribe when being destroyed
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

    // Update the text delay when settings is changed
    private void UpdateDelay(float newDelay)
    {
        delay = newDelay;
    }
}
