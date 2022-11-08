using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Note</c> that encapsulates a note.
/// Updates its position to emulate falling, and maintains when it is supposed to be pressed.
/// Note that the beats are given in seconds, while the leeway is given in beats.
/// </summary>
public class Note : MonoBehaviour
{
    /* Some hard coded constants, such as start position and target end position */
    public static float FALL_LINE = -2f;
    public static Vector3 START_POS = new Vector3(0f,5f,0f);
    public static Vector3 TARGET = new Vector3(0f,FALL_LINE,0f);

    /* Constants that determine drop speed and time */
    public static float fallTime = 2f;
    public static float fallSpeed = Vector3.Distance(TARGET, START_POS) / fallTime;

    /* The default amount of leeway we allow the player, in beats */
    public static float DEFAULT_LEEWAY = .125f;

    /* External objects */
    private Player player;
    private BeatController controller;

    /* Times when the note starts and ends, in seconds */
    private float beatStart;
    private float beatEnd;

    /* How long this note has been alive for, used for lerping */
    private float lifetime;

    /// <summary>
    /// Method <c>SetInitialState</c> sets start and end times for beats, and the current lifetime of the note.
    ///     Lifetime has to be set because Update(s) aren't completely synced with the beat of the music.
    ///     That is to say, the controller usually spawns a note ~0.02 seconds after it should spawn.
    /// Also sets the y-scale such that the note's size is exactly when the user can hit it.
    /// </summary>
    public void SetInitialState (float start, float end, float time)
    {
        beatStart = start;
        beatEnd = end;
        lifetime = time;
        this.transform.localScale = new Vector3(1, (beatEnd - beatStart) * controller.secondsPerBeat * fallSpeed * 2, 1);
    }

    // Lerps and checks destruction
    void Update()
    {
        lifetime += Time.deltaTime;
        
        lerpPosition();

        if (this.transform.position.y + this.transform.localScale.y / 2f < FALL_LINE)
            DestroyNote();
    }

    // Destroys note and resets player combo
    private void DestroyNote()
    {
        if (player) player.resetPlayerCombo();
        if (controller) controller.DequeueFrontNote();
        GameObject.Destroy(this.gameObject);
    }

    // Lerps the note
    private void lerpPosition()
    {
        // We use `LerpUnclamped` so that the note can go just past the target without stopping
        // and then get destroyed
        this.transform.position = new Vector3(
            Mathf.LerpUnclamped(START_POS.x, TARGET.x, lifetime / fallTime),
            Mathf.LerpUnclamped(START_POS.y, TARGET.y, lifetime / fallTime),
            Mathf.LerpUnclamped(START_POS.z, TARGET.z, lifetime / fallTime)
        );
    }

    /* Getters and Setters */ 
    public float getBeatStart() { return beatStart; }
    public float getBeatEnd() { return beatEnd; }
    public void SetPlayer(Player player) { this.player = player; }
    public void SetController(BeatController controller) { this.controller = controller; }
    public void SetLifetime(float time) { this.lifetime = time; }
}
