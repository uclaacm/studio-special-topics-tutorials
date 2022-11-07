using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    /* Predetermined, based on the song and the created beatmap */
    [Header("Song Information")]
    [Tooltip("The song BPM")]
    [SerializeField] private float bpm;

    [Tooltip("The offset, in beats, until song starts")]
    [SerializeField] private float songStartOffset;

    [Tooltip("Time to end of song, in seconds, not including any offset")]
    [SerializeField] private float endTime;

    /* Various external components */
    [Header("External Components")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Player player;

    [SerializeField] private AudioCueEventChannelSO globalAudioMessenger;
    private StudioLand.MinigameController minigameController;

    /* Calculated once when game Controller loads */
    [Header("Debug Fields")]
    public float startTime;
    public float beatsPerSecond;
    public float secondsPerBeat;

    public float debugClock;

    /* Calculated every Update() */
    public float songPosition;
    public float songPositionInBeats;

    /* Component fields */
    private AudioSource audioSource;

    /* Fields to set up notes */
    private Queue<(float, float)> beatmap = new Queue<(float, float)>();
    private Queue<Note> currentlyLiveNotes = new Queue<Note>();

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();

        beatsPerSecond = bpm / 60;
        secondsPerBeat = 1 / beatsPerSecond;
        startTime = (float)AudioSettings.dspTime + songStartOffset * secondsPerBeat;
        endTime += songStartOffset * secondsPerBeat;
        SetBeatmap();
        debugClock = 1;

        globalAudioMessenger.RaiseStopEvent(new AudioCueKey(0, null));
    }

    void Start()
    {
        minigameController = GameObject.Find("Minigame Controller").GetComponent<StudioLand.MinigameController>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        songPosition = ((float)AudioSettings.dspTime - startTime);
        if (songPosition >= startTime && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        songPositionInBeats = beatsPerSecond * songPosition;
        float start, middle, end;
        Note note;

        ClearNullNotes();

        if (beatmap.Count > 0) 
        {
            (start, end) = beatmap.Peek();
            middle = (end + start) / 2f;

            if(songPosition >= middle - Note.fallTime)
            {
                note = GameObject.Instantiate(notePrefab).GetComponent<Note>();
                note.player = player;
                note.controller = this;
                note.SetInitialState(start, end, (songPosition - (middle - Note.fallTime)));
                currentlyLiveNotes.Enqueue(note);
                beatmap.Dequeue();
            }
        }

        if (songPosition >= endTime)
        {
            minigameController.SetGameScore((float)GameObject.Find("Player").GetComponent<Player>().getScore());
            minigameController.EndGame();
        }
        GenerateDebugLine();
    }

    public Queue<Note> getCurrentlyLiveNotes() { return currentlyLiveNotes; }
    public void DequeueFrontNote() { currentlyLiveNotes.Dequeue(); }

    public void SetBeatmap()
    {
        beatmap.Clear(); 
        for (float i = 1; i < 31; i ++)
        {
            beatmap.Enqueue(((i - Note.DEFAULT_LEEWAY) * secondsPerBeat, (i + Note.DEFAULT_LEEWAY) * secondsPerBeat));
        }
    }

    public void ClearNullNotes()
    {
        Note note;
        while (currentlyLiveNotes.Count > 0)
        {
            note = currentlyLiveNotes.Peek();
            if (note == null)
            {
                currentlyLiveNotes.Dequeue();
            }
            else
            {
                return;
            }
        }
    }

    public void GenerateDebugLine()
    {
        if (songPosition >= debugClock * secondsPerBeat - Note.fallTime)
        {
            Note note = GameObject.Instantiate(notePrefab).GetComponent<Note>();
            note.transform.localScale = new Vector3(100f, .01f, 1);
            note.lifetime = songPosition - (debugClock * secondsPerBeat - Note.fallTime); 
            debugClock += 1;
        }
    }
}
