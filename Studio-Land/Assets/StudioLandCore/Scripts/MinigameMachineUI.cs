using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class MinigameMachineUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TMPro.TMP_Text titleText;
        [SerializeField] TMPro.TMP_Text scoreText;

        [Header("Colors")]
        [SerializeField] Color invalidColor = Color.red;
        [SerializeField] Color scoreColor = Color.green;
        [SerializeField] Color titleColor = Color.yellow;

        void Awake()
        {
            // Default invalid state
            titleText.text = "OUT OF ORDER";
            titleText.color = invalidColor;

            scoreText.text = "";
            scoreText.color = scoreColor;
        }

        // This is a public function call as opposed to a private handler because it's easier to hook up and
        // understand with a UnityEvents invocation (no need for complex handler set up)
        public void UpdateScreen(MinigameSO minigameSO)
        {
            // Format = set text, then set color (make sure every time update happens, state is in a valid configuration)
            titleText.text = minigameSO.title;
            titleText.color = titleColor;

            scoreText.text = $"SCORE: {minigameSO.highestScore}";
            scoreText.color = scoreColor;
        }
    }
}

