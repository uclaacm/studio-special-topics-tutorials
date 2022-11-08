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
    [SerializeField] private AudioCueEventChannelSO globalAudioMessenger;
    [SerializeField] private AudioCueSO audioCue;
    [SerializeField] private AudioConfigurationSO audioConfig;

    private StudioLand.MinigameController minigameController;

    /* Fields to set up notes */
    private Queue<(float, float)> beatmap = new Queue<(float, float)>();
    private Queue<Note> currentlyLiveNotes = new Queue<Note>();

    void Awake()
    {
        /** TODO #1
         * Implement the setup for tracking the two units of time,
         *      as well as setting up other constants as needed
         * If you're not sure where to start,
         *      check out the "Debug Fields" above,
         * and see which ones are calculated on load
         * Remember to keep track of your units!
         */

        /* END TODO */

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
        /** TODO #2
         * Update the song position... again, look at the fields and see what's not implemented yet
         */

        songPosition = float.NaN;
        /* END TODO */

        ClearNullNotes();
        GenerateDebugLine();

        /** TODO #3
         * Load the notes from the beatmap as needed
         *
         * To get you started, you want to check when a note should be hit, 
         *      and keep in mind it takes some time from the point a note spawns and the point it lands at the fall line
         *
         * Look into GameObject.Instantiate,
         * the C# Queue documentation (if you're not a cs kinda person, the important methods are Peek, Enqueue, and Dequeue),
         * and a little bit into the implementation of Note.
         *
         */

        float start, middle, end;
        // This can be a while loop too, but since the beatmap as implemented isn't ever going to spawn two notes at once, I left is as an if
        if (beatmap.Count > 0) 
        {
            // Gets the start and end time of the next note
            (start, end) = beatmap.Peek();
            middle = (end + start) / 2f;

            /* Enter logic here */
        }

        /* END TODO */

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

    /* Helpers */
    /**
     * You don't have to really look at these if you don't want to...
     * The most relevant one is SetBeatmap, if you want to customize what the beat map looks like
     */

    // Creates the beat map
    // Note that the i tracks beats, but the beatmap itself is converted to
    private void SetBeatmap()
    {
        beatmap.Clear(); 
        for (float i = 3; i < 31; i ++)
        {
            beatmap.Enqueue(((i - Note.DEFAULT_LEEWAY) * secondsPerBeat, (i + Note.DEFAULT_LEEWAY) * secondsPerBeat));
        }
    }

    // A backup to ensure that the next note that is pulled is a non-null note, since notes can be destroyed.
    // Theoretically, if note destruction is handled properly this should not be necessary.
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
        globalAudioMessenger.RaiseStopEvent(AudioCueKey.Invalid);
        StartCoroutine(PlayMusicWithOffset());
    }

    private IEnumerator PlayMusicWithOffset()
    {
        yield return new WaitForSeconds(songStartOffsetInSeconds);
        globalAudioMessenger.RaisePlayEvent(audioCue, audioConfig, Vector3.zero);
    }
    
    private void OnDestroy()
    {
        sceneReadyChannelSO.OnEventRaised -= HandleSceneReadied;
    }
}
