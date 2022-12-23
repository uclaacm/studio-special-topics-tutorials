using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

[RequireComponent(typeof(Image), typeof(AudioSource))]
public class NPC : MonoBehaviour
{
    [SerializeField] Yarn.Program script;
    [SerializeField] Color inactiveColor = Color.gray;

    static Image background;
    static Image blackScreen;
    Image image;
    AudioSource audioSource;
    Transform parent;

    // Awake is called super early
    void Awake()
    {
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        parent = transform.parent;

        background = GameObject.Find("Background").GetComponent<Image>();
        blackScreen = GameObject.Find("Black Screen").GetComponent<Image>();
    }

    /// <summary> Sets the sprite image of the gameObject. </summary>
    [YarnCommand("setSprite")]
    public IEnumerator<WaitForSeconds> SetSprite(string spritePath, float fadeTime = 0.5f)
    {
        if ((image.sprite = Resources.Load<Sprite>(spritePath)) == null)
        {
            Debug.LogError("Couldn't find character sprite in " + spritePath);
            yield break;
        }

        if (image.color.a <= float.Epsilon || !image.enabled)
        {
            image.enabled = true;
            image.DOFade(1f, fadeTime);
            yield return new WaitForSeconds(fadeTime);
        }        
    }
    
    /// <summary> Clears the gameObject's sprite image. </summary>
    [YarnCommand("clearSprite")]
    public IEnumerator<WaitForSeconds> ClearSprite(float fadeTime = 0.5f)
    {
        image.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        image.enabled = false;
        image.sprite = null;
    }

    /// <summary> Brings the gameObject to the front by darkening other sprites. </summary>
    [YarnCommand("setActiveChar")]
    public void SetActiveChar()
    {
        transform.SetSiblingIndex(3);
        image.DOColor(Color.white, 0.5f);
        // Last child is the bottom sprite, which will never be inactive
        parent.GetChild(0).GetComponent<Image>().DOColor(new Color(inactiveColor.r, inactiveColor.g, inactiveColor.b, parent.GetChild(0).GetComponent<Image>().color.a), 0.5f);
        parent.GetChild(1).GetComponent<Image>().DOColor(new Color(inactiveColor.r, inactiveColor.g, inactiveColor.b, parent.GetChild(1).GetComponent<Image>().color.a), 0.5f);
        parent.GetChild(2).GetComponent<Image>().DOColor(new Color(inactiveColor.r, inactiveColor.g, inactiveColor.b, parent.GetChild(2).GetComponent<Image>().color.a), 0.5f);
    }

    /// <summary> Plays a sound from the gameObject. </summary>
    [YarnCommand("playSound")]
    public void PlaySound(string soundPath)
    {
        if ((audioSource.clip = Resources.Load<AudioClip>(soundPath)) == null)
        {
            // Debug.LogError("Couldn't find audio clip in " + soundPath);
            return;
        }
        audioSource.Play();
    }

    [YarnCommand("flip")]
    public void Flip(float seconds = 0f)
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, -180, 0), seconds);
    }

    [YarnCommand("shake")]
    public void Shake(float seconds = 1f, float strength = 10f)
    {
        transform.DOShakePosition(seconds, strength);
    }

    [YarnCommand("setColor")]
    public void SetColor(float red, float green, float blue, float alpha = 1f)
    {
        image.color = new Color(red, green, blue, alpha);
    }
    [YarnCommand("setColorHex")]
    public void SetColorHex(string hexCode)
    {
        Color newColor = new Color();
        ColorUtility.TryParseHtmlString(hexCode, out newColor);
        image.color = newColor;
    }
}
