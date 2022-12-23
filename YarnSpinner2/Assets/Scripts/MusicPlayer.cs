using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static AudioSource audioSource;
    
    private void Awake()
    {
        if (audioSource)
        {
            MusicPlayer.audioSource.clip = Resources.Load<AudioClip>("Music/Title");
            MusicPlayer.audioSource.Play();
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }
}