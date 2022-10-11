using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace StudioLand
{
    public class ScoreUI : PanelUI
    {
        [SerializeField] AssetReference mainScene;

        [Header("Broadcasts on")]
        [SerializeField] LoadEventChannelSO requestLoadSceneChannel;
        

        public void Initialize(float data)
        {
            root.Q<Button>("Okay").RegisterCallback<ClickEvent>(ev => requestLoadSceneChannel.RaiseEvent(mainScene));
            root.Q<Label>("Score").text = "Score: " + (int)data;
        }

        public override bool CanDeselect => false;
    }
}

