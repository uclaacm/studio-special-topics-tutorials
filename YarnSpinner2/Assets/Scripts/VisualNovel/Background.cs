using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

[RequireComponent(typeof(Image), typeof(AudioSource))]
public class Background : MonoBehaviour
{
    private static Background _instance;
    public static Background Instance { get { return _instance; } }

    private Image background;

    public AudioSource audioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
            background = GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
        }
    }

    /// <summary> Changes background sprite. </summary>
    [YarnCommand("SetBackground")]
    public static void SetBackground(string bgPath)
    {
        if ((_instance.background.sprite = Resources.Load<Sprite>(bgPath)) == null)
        {
            Debug.LogError("Couldn't find background in " + bgPath);
        }
    }
}
