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
    [Header("External Objects")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private Player player;

    [SerializeField] private VoidEventChannelSO sceneReadyChannelSO;
    [SerializeField] private AudioCueEventChannelSO globalMusicMessenger;
    [SerializeField] private AudioCueEventChannelSO globalSFXMessenger;
    [SerializeField] private AudioCueSO musicCue;
    [SerializeField] private AudioCueSO beatCue;
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
        GenerateDebugLine();

        if (beatmap.Count > 0) 
        {
            (start, end) = beatmap.Peek();
            middle = (end + start) / 2f;

            if(songPosition >= middle - Note.fallTime)
            {
                note = GameObject.Instantiate(notePrefab).GetComponent<Note>();
                note.SetPlayer(player);
                note.SetController(this);
                note.SetInitialState(start, end, ((middle - Note.fallTime) - songPosition));
                currentlyLiveNotes.Enqueue(note);
                beatmap.Dequeue();
            }
        }

        minigameController.SetGameScore((float)GameObject.Find("Player").GetComponent<Player>().getScore());
        if (songPosition >= endTime)
        {
            minigameController.SetGameScore((float)GameObject.Find("Player").GetComponent<Player>().getFinalScore());
            minigameController.EndGame();
        }
    }

    /* Getters / Setters */
    public Queue<Note> getCurrentlyLiveNotes() { return currentlyLiveNotes; }
    public void DequeueFrontNote() { currentlyLiveNotes.Dequeue(); }
    public void PlayHit() { globalSFXMessenger.RaisePlayEvent(beatCue, audioConfig, Vector3.zero); }
    /* Helpers */

    // Creates the beat map
    // Note that the i tracks beats, but the beatmap itself is converted to seconds
    private void SetBeatmap()
    {
        beatmap.Clear(); 
        for (float i = 3; i < 31; i ++)
        {
            beatmap.Enqueue(((i - Note.DEFAULT_LEEWAY) * secondsPerBeat, (i + Note.DEFAULT_LEEWAY) * secondsPerBeat));
        }
    }

    // A backup to ensure that the next note that is pulled is a non-null note, since notes can be destroyed.
    // Theoretically, if notes are handled properly this should not be necessary.
    private void ClearNullNotes()
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

    // Generates the debug lines at every beat
    private void GenerateDebugLine()
    {
        if (songPosition >= debugClock * secondsPerBeat - Note.fallTime)
        {
            Note note = GameObject.Instantiate(notePrefab).GetComponent<Note>();
            note.transform.localScale = new Vector3(100f, .01f, 1);
            note.SetLifetime((debugClock * secondsPerBeat - Note.fallTime) - songPosition); 
            debugClock += 1;
        }
    }
    
    /* Various setup to handle audio and music */
    private void HandleSceneReadied()
    {
        globalMusicMessenger.RaiseStopEvent(AudioCueKey.Invalid);
        StartCoroutine(PlayMusicWithOffset());
    }

    private IEnumerator PlayMusicWithOffset()
    {
        yield return new WaitForSeconds(songStartOffsetInSeconds);
        globalMusicMessenger.RaisePlayEvent(musicCue, audioConfig, Vector3.zero);
    }
    
    private void OnDestroy()
    {
        sceneReadyChannelSO.OnEventRaised -= HandleSceneReadied;
    }
}
