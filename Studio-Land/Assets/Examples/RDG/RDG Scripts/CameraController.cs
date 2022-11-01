using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{

    public Room currRoom;
    public float camSpeed;

    [SerializeField] Camera camera;
    [SerializeField] RenderPipelineAsset renderer2D;

    public static CameraController instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            Debug.Log("camera created");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        MoveCam();
    }

    private void MoveCam()
    {

        if (currRoom == null)
        {
            Debug.Log("current room not set");
            return;
        }

        /* =============

        SLIDE 24

        Task:
        - Snap camera to room player is currently in (currRoom, will be set later in Room script)

        EXAMPLE CODE:

        Vector3 newPos = new Vector3(currRoom.X * currRoom.width, currRoom.Y * currRoom.height);
        newPos.z = transform.position.z;

        transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * camSpeed);

        ============= */

    }
}
