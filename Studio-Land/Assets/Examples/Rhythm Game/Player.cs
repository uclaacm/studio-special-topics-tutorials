using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int combo;
    private BeatController controller;
    private TMP_Text scoreText;

    void Start()
    {
        score = 0;
        combo = 0;
        controller = GameObject.Find("Audio Source").GetComponent<BeatController>();
        scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
        updateText();
    }

    void Update()
    {
        updateText();
    }

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

    public void HitNote(Note note)
    {
        score += 1;
        combo += 1;
        Destroy(note.gameObject);
        controller.DequeueFrontNote();
    }

    public int getScore() { return score; }
    public void resetPlayerCombo() { combo = 0; updateText(); }
    private void updateText() { scoreText.text = $"Score: {score}\nCombo: {combo}\ncurBeat: {controller.songPositionInBeats}"; }
}
