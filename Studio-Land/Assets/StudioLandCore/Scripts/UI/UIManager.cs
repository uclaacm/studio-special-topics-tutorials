using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] List<PanelUI> uiPanels;
        [SerializeField] InputReaderSO playerInput;
        PanelUI currentOpenPanel;

        [Header("Listens on")]
        [SerializeField] VoidEventChannelSO sceneReadyEventChannel;

        void Awake()
        {
            sceneReadyEventChannel.OnEventRaised += HandleSceneReadied;
            
            foreach(var panel in uiPanels)
            {
                // panel.OnActivate += () => HandlePanelActivation(panel);
                // panel.OnDeactivate += () => HandlePanelDeactivation(panel);
            }
        }

        void HandleSceneReadied()
        {
            playerInput.EnableGameplayInput();
            currentOpenPanel?.Reset();
        }

        void HandlePanelActivation(PanelUI panel)
        {
            playerInput.EnableUIInput();
            currentOpenPanel = panel;
        }

        void HandlePanelDeactivation(PanelUI panel)
        {
            if(panel != currentOpenPanel) {return;}

            playerInput.EnableGameplayInput();
            currentOpenPanel?.AnimateOut();
            currentOpenPanel = null;
        }
    }
}

