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

        /* ==========
        YOUR CODE HERE (Slide 24)
        ========== */

    }

    
}
