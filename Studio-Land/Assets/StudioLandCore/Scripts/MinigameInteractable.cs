using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    /// <summary>
    /// This is an interactable made specifically for the interaction that brings up the instructions panel.
    /// Note that this is a HARD CODED component tailored specifically for this interaction and would not be
    /// usable if we ever decided to remove the instructions panel.
    /// </summary>
    public class MinigameInteractable : Interactable
    {
        [SerializeField] MinigameRequester minigameRequester;
        [SerializeField] VoidEventChannelSO instructionsPanelClosedEventChannel;    // When the instruction panel closes, interaction over
        
        void Awake()
        {
            instructionsPanelClosedEventChannel.OnEventRaised += HandleInstructionsPanelClosed;
        }

        void OnDestroy()
        {
            instructionsPanelClosedEventChannel.OnEventRaised -= HandleInstructionsPanelClosed;
        }
        
        public override void ExecuteInteraction()
        {
            minigameRequester.PlayMinigame();
        }

        void HandleInstructionsPanelClosed()
        {
            FinishInteraction();
        }
    }
}

