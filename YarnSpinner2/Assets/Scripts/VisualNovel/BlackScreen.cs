using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

[RequireComponent(typeof(Image))]
public class BlackScreen : MonoBehaviour
{
    private static BlackScreen _instance;
    public static BlackScreen Instance { get { return _instance; } }

    public Image blackScreen;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } 
        else 
        {
            _instance = this;
            blackScreen = GetComponent<Image>();
        }
    }

    public void Fade(bool fadeIn, float seconds = 1f, string colorHex = "#000000")
    {
        StartCoroutine(FadeHelper(fadeIn, seconds, colorHex));
    }
    /// <summary> 
    /// Fades a colored overlay over a specified amount of seconds. 
    /// If FadeIn is true, then it fades the scene in. If FadeIn is false, then it fades the scene out into color. 
    /// Seconds defaults to 1. HexColor defaults to "#000000" (Black). 
    /// Text will not show over the color screen. 
    /// </summary>
    [YarnCommand("fade")]
    public static IEnumerator FadeHelper(bool fadeIn, float seconds = 1f, string colorHex = "#000000")
    {
        colorHex += fadeIn ? "00" : "ff";
        Color newColor = Color.black;
        ColorUtility.TryParseHtmlString(colorHex, out newColor);
        
        if (_instance.blackScreen.color != newColor)
        {
            _instance.blackScreen.color = new Color(newColor.r, newColor.g, newColor.b, _instance.blackScreen.color.a);

            _instance.blackScreen.DOFade(fadeIn ? 0f : 1f, seconds);
            yield return new WaitForSeconds(seconds);
        }
    }
}
