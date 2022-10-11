using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class FadeManager : MonoBehaviour
{
    [SerializeField] UIDocument blackScreen;
    [SerializeField] FloatEventChannelSO fadeInRequestChannel;
    [SerializeField] FloatEventChannelSO fadeOutRequestChannel;

    void OnEnable()
    {
        //blackScreen.sortingOrder = 0;
        fadeInRequestChannel.OnEventRaised += HandleFadeInRequested;
        fadeOutRequestChannel.OnEventRaised += HandleFadeOutRequested;
    }

    void OnDisable()
    {
        fadeInRequestChannel.OnEventRaised -= HandleFadeInRequested;
        fadeOutRequestChannel.OnEventRaised -= HandleFadeOutRequested;
    }

    void HandleFadeInRequested(float duration)
    {
        //blackScreen.sortingOrder = 0;      // Prevent invisible black screen from blocking raycasts
        StyleValues newStyle = new StyleValues {opacity = 0};
        blackScreen.rootVisualElement.Q("Black").experimental.animation.Start(newStyle, (int)(duration * 1000));
        //blackScreen.DOFade(0, duration);
    }

    void HandleFadeOutRequested(float duration)
    {
        //blackScreen.sortingOrder = 99;
        StyleValues newStyle = new StyleValues {opacity = 1};
        blackScreen.rootVisualElement.Q("Black").experimental.animation.Start(newStyle, (int)(duration * 1000));
        //blackScreen.DOFade(1, duration);
    }
}
