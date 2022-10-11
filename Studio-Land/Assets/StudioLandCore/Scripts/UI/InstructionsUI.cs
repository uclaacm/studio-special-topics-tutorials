using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace StudioLand
{
    public class InstructionsUI : PanelUI
    {
        [Header("Broadcasts on")]
        [SerializeField] LoadEventChannelSO requestLoadSceneChannel;
        public override bool CanDeselect => true;

        public void Initialize(MinigameSO minigame)
        {           
            root.Q<Label>("Title").text = minigame.title;

            root.Q<Label>("Description").text = minigame.instructions;

            StyleBackground pic = new StyleBackground();
            pic.value = Background.FromSprite(minigame.picture);

            root.Q("Picture").style.backgroundImage = pic;

            root.Q<Button>("Play").RegisterCallback<ClickEvent>(ev => PlayMinigame(minigame));

            
        }

        void PlayMinigame(MinigameSO minigame)
        {
            requestLoadSceneChannel.RaiseEvent(minigame.scene);
        }
        
    }
}

