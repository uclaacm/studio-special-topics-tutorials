using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class owo : MonoBehaviour
{

    //i luv tamagoyaki feed me owo :)))

    [SerializeField] VideoClip uwu;
    
    private float x;

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "potowo"){
            Debug.Log("owo");
            
            var videoPlayer = CameraController.instance.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.playOnAwake = true;
            videoPlayer.isLooping = false;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
            videoPlayer.clip = uwu;

            GameManager.instance.omo();
        }
    }


}
