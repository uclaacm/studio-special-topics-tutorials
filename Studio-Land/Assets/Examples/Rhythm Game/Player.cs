using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    
    // [SerializeField] private float jumpTime;
    // private bool jumping;
    // private float timer;

    [SerializeField] private int score;
    [SerializeField] private int combo;
    private BeatController controller;
    private TMP_Text scoreText;

    void Start()
    {
        // timer = 0f;
        // jumping = false;
        score = 0;
        combo = 0;
        controller = GameObject.Find("Audio Source").GetComponent<BeatController>();
        scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        updateText();
    }

    public void OnPress(InputValue value)
    {
        float curBeat = controller.songPositionInBeats;
        Note nearestNote;
        if (controller.getCurrentlyLiveNotes().Count > 0) {
            nearestNote = controller.getCurrentlyLiveNotes().Peek();
            float diff = Mathf.Abs(curBeat - nearestNote.beat);
            // Implement handle to avoid spam press
            if (diff < Note.LEEWAY) {
                // jumping = true;
                score += 1;
                combo += 1;
                Destroy(nearestNote.gameObject);
            }
            if (nearestNote.destroyBeat <= curBeat) {
                combo = 0;
            }
        }
        updateText();
    }

    public int getScore() { return score; }
    public void resetPlayerCombo() { combo = 0; updateText(); }
    private void updateText() { scoreText.text = $"Score: {score}\nCombo: {combo}"; }
}
