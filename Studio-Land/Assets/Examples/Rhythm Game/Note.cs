using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static float FALL_LINE = -2f;
    public static Vector3 START_POS = new Vector3(0f,5f,0f);
    public static Vector3 TARGET = new Vector3(0f,FALL_LINE,0f);
    public static float DEFAULT_LEEWAY = .125f;

    public static float fallTime = 2f;
    public static float fallSpeed = Vector3.Distance(TARGET, START_POS) / fallTime;

    public Player player;
    public BeatController controller;

    public float lifetime;

    /* Times when the note starts and ends, in seconds */
    private float beatStart;
    private float beatEnd;

    public float getBeatStart() { return beatStart; }
    public float getBeatEnd() { return beatEnd; }
    public bool isHold() { return (beatEnd - beatStart) > (2 * DEFAULT_LEEWAY); }

    public void SetInitialState (float start, float end, float time) {
        beatStart = start;
        beatEnd = end;
        lifetime = time;
        this.transform.localScale = new Vector3(1, (beatEnd - beatStart) * controller.secondsPerBeat * fallSpeed, 1);
    }

    void Update()
    {
        lifetime += Time.deltaTime;
        
        lerpPosition();

        if (this.transform.position.y + this.transform.localScale.y / 2f < FALL_LINE)
        {
            DestroyNote();
        }
    }

    private void DestroyNote()
    {
        if (player) player.resetPlayerCombo();
        if (controller) controller.DequeueFrontNote();
        GameObject.Destroy(this.gameObject);
    }

    private void lerpPosition()
    {
        float nextX, nextY, nextZ;
        
        nextX = Mathf.LerpUnclamped(START_POS.x, TARGET.x, lifetime / fallTime);
        nextY = Mathf.LerpUnclamped(START_POS.y, TARGET.y, lifetime / fallTime);
        nextZ = Mathf.LerpUnclamped(START_POS.z, TARGET.z, lifetime / fallTime);
        this.transform.position = new Vector3(nextX, nextY, nextZ);
    }
}
