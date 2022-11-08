using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Class <c>Player</c> that handles player input, score, and the score UI.
/// </summary>
public class Player : MonoBehaviour
{
    /* Fields for scoring */
    private int score;
    private int combo;
    private int maxCombo;

    /* External objects */
    private BeatController controller;
    private TMP_Text scoreText;

    void Awake()
    {
        score = 0;
        combo = 0;
        maxCombo = 0;
        controller = GameObject.Find("Audio Source").GetComponent<BeatController>();
        scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
    }

    void Update()
    {
        updateText();
    }

    // Event that is called whenever player presses "Spacebar"
    public void OnPress(InputValue value)
    {
        float curTime = controller.songPosition;
        Note nearestNote;
        float beatStartTime, beatEndTime;
        
        if (controller.getCurrentlyLiveNotes().Count > 0)
        {
            nearestNote = controller.getCurrentlyLiveNotes().Peek();
            beatStartTime = nearestNote.getBeatStart();
            beatEndTime = nearestNote.getBeatEnd();

            if (beatStartTime <= curTime && curTime <= beatEndTime)
            {
                HitNote(nearestNote);
            }
        }
        updateText();
    }

    /* Helpers */
    
    // Increments score and destroys the hit note
    private void HitNote(Note note)
    {
        score += 1;
        combo += 1;
        Destroy(note.gameObject);
        controller.DequeueFrontNote();
    }

    private void updateText()
    {
        scoreText.text = $"Score: {score}\nCombo: {combo}\ncurBeat: {controller.songPositionInBeats}";
    }
    
    /* Getters and Setters */
    public int getScore() { return score; }
    public int getFinalScore() { return score * maxCombo; }
    public void resetPlayerCombo() { if (combo > maxCombo) maxCombo = combo; combo = 0; updateText(); }
}
