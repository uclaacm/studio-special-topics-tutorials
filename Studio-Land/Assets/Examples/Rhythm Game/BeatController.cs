using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    [SerializeField] private float bpm;

    /* Calculated once when game Controller loads */
    public float startTime;
    public float beatsPerSecond;
    public float secondsPerBeat;
    [Tooltip("Time to end of song, in seconds, not including any offset")]
    public float endTime;
    /* Calculated every Update() */
    public float songPosition;
    public float songPositionInBeats;
    public float songStartOffset;

    /* Component fields */
    private AudioSource audioSource;

    /* Fields to set up note drop*/
    private Queue<float> beatmap = new Queue<float>();
    private Queue<Note> currentlyLiveNotes = new Queue<Note>();
    
    public GameObject notePrefab;
    public FloatEventChannelSO scoreMessenger;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();

        beatsPerSecond = bpm / 60;
        secondsPerBeat = 1 / beatsPerSecond;
        startTime = (float)AudioSettings.dspTime + songStartOffset;
        endTime += songStartOffset;
        audioSource.Play();

        for (float i = 6; i < 31; i ++) {
            beatmap.Enqueue(i);
        }
    }

    void Update()
    {
        songPosition = ((float)AudioSettings.dspTime - startTime);
        songPositionInBeats = beatsPerSecond * songPosition;
        float notePos;
        Note note;

        while (currentlyLiveNotes.Count > 0) {
            note = currentlyLiveNotes.Peek();
            if (note == null || note.destroyBeat < songPositionInBeats)
                currentlyLiveNotes.Dequeue();
            else
                break;
        }

        if (beatmap.Count > 0){
            notePos = beatmap.Peek();
            if(songPositionInBeats >= notePos - Note.fallTime * beatsPerSecond) {
                note = GameObject.Instantiate(notePrefab).GetComponent<Note>();
                note.beat = notePos;
                currentlyLiveNotes.Enqueue(note);
                beatmap.Dequeue();
            }
        }

        if (songPosition >= endTime) {
            scoreMessenger.RaiseEvent((float)GameObject.Find("Player").GetComponent<Player>().getScore());
        }
    }

    public Queue<Note> getCurrentlyLiveNotes() { return currentlyLiveNotes; }
}
