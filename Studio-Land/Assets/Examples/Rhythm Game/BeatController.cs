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

    /* Calculated once when game Controller loads */
    [Header("Debug Fields")]
    [Tooltip("Time to when the song will start, including offset")]
    public float startTime;
    [Tooltip("BPM / 60")]
    public float beatsPerSecond;
    [Tooltip("1 / BPS (or equivalently, 60 / BPM")]
    public float secondsPerBeat;
    [Tooltip("Offset in seconds until song starts (simply converted from songStartOffset")]
    public float songStartOffsetInSeconds;

    [Tooltip("Clock that ticks (approximately) every beat")]
    public float debugClock;

    /* Calculated every Update() */
    [Tooltip("Song position in seconds")]
    public float songPosition;
    [Tooltip("Song position in beats")]
    public float songPositionInBeats;

    /* Various external components */
    [Header("External Components")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Player player;

    [SerializeField] private VoidEventChannelSO sceneReadyChannelSO;
    [SerializeField] private AudioCueEventChannelSO globalAudioMessenger;
    [SerializeField] private AudioCueSO audioCue;
    [SerializeField] private AudioConfigurationSO audioConfig;

    private StudioLand.MinigameController minigameController;

    /* Fields to set up notes */
    private Queue<(float, float)> beatmap = new Queue<(float, float)>();
    private Queue<Note> currentlyLiveNotes = new Queue<Note>();

    void Awake()
    {
        beatsPerSecond = bpm / 60;
        secondsPerBeat = 1 / beatsPerSecond;
        songStartOffsetInSeconds = songStartOffset * secondsPerBeat;

        startTime = (float)AudioSettings.dspTime + songStartOffsetInSeconds;
        endTime += songStartOffset * secondsPerBeat;

        SetBeatmap();
        debugClock = 1;
    }

    void Start()
    {
        sceneReadyChannelSO.OnEventRaised += HandleSceneReadied;
        
        minigameController = GameObject.Find("Minigame Controller").GetComponent<StudioLand.MinigameController>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        songPosition = ((float)AudioSettings.dspTime - startTime);
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
        for (float i = 3; i < 31; i ++)
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
    
    private void HandleSceneReadied()
    {
        globalAudioMessenger.RaiseStopEvent(AudioCueKey.Invalid);

        StartCoroutine(PlayMusicWithOffset());
    }

    private IEnumerator PlayMusicWithOffset()
    {
        yield return new WaitForSeconds(songStartOffsetInSeconds);
        globalAudioMessenger.RaisePlayEvent(audioCue, audioConfig, Vector3.zero);
    }
}
