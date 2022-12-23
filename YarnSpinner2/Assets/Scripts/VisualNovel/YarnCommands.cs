using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using DG.Tweening;

/// <summary> Static Yarn Spinner commands. Commands either return nothing or are coroutines </summary>
public class YarnCommands : MonoBehaviour
{
    /// <summary> Fades the background to a new one over time. </summary>
    [YarnCommand("fadeBackground")]
    public static IEnumerator<WaitForSeconds> FadeBackground(string bgPath, float seconds = 2f)
    {
        float halvedDuration = seconds / 2;
        
        BlackScreen.Instance.Fade(false, halvedDuration);
        yield return new WaitForSeconds(halvedDuration);
        Background.SetBackground(bgPath);
        BlackScreen.Instance.Fade(true, halvedDuration);
        yield return new WaitForSeconds(halvedDuration);
    }

    /// <summary> Plays a song in the background. </summary>
    [YarnCommand("playMusic")]
    public static void PlayMusic(string musicPath)
    {
        AudioClip musicClip = Resources.Load<AudioClip>(musicPath);
        if ((MusicPlayer.audioSource.clip = musicClip) == null)
        {
            // Debug.LogError("Couldn't find audio clip in " + introPath);
            return;
        }
        MusicPlayer.audioSource.loop = true;
        MusicPlayer.audioSource.Play();
    }

    /// <summary> Stops music in the background. </summary>
    [YarnCommand("stopMusic")]
    public static void StopMusic()
    {
        MusicPlayer.audioSource.Stop();
    }

    /// <summary> Loads a new scene. Make sure the scene is in Build Settings! </summary>
    [YarnCommand("loadScene")]
    public static void LoadScene(string scene)
    {
        BlackScreen.Instance.blackScreen.DOFade(1f, 0.5f).OnComplete(() => SceneManager.LoadScene(scene));
        PlayerPrefs.Save();
    }

    /// <summary> Changes your cursor to something EPIC. </summary>
    [YarnCommand("dwayne")]
    public static void Dwayne()
    {
        Cursor.SetCursor(Resources.Load<Texture2D>("Dwayne/Running"), new Vector2(18, 0), CursorMode.Auto);
    }

    // Add more useful commands here!
}
