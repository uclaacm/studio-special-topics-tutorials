using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public static float distance = 7f;
    public static float fallTime = 2f;
    public static float fallSpeed = distance / fallTime;
    public static float FALL_LINE = -2f;
    public static float LEEWAY = .25f;

    public float beat;
    public float destroyBeat;

    void Start()
    {
        destroyBeat = beat + LEEWAY;
    }
    
    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - fallSpeed * Time.deltaTime, this.transform.position.z);
        if (this.transform.position.y < FALL_LINE) {
            GameObject.Destroy(this.gameObject);
            GameObject.Find("Player").GetComponent<Player>().resetPlayerCombo();
        }
    }
}
