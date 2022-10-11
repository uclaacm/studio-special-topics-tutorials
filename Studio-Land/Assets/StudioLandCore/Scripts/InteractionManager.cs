using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioLand
{
    /// <summary>
    /// Script for ensuring interactions are processed one at a time. As with most managers, the Interaction Manager
    /// serves to unify its units (interactables) under a common network to prevent the units from interfering with
    /// each other. It keeps the interactables "respectful" of one another. For example, if one interactable is currently
    /// being activated, no other interactable under the interaction manager can activate.
    /// </summary>
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] InputReaderSO inputReader;
        [SerializeField] InteractablesRuntimeSetSO interactablesRuntimeSet;
        [SerializeField] TransformAnchor interactorTransformAnchor;     // This will be the player, usually
        Interactable currentInteractable;
        bool isInteractionEnabled = true;

        [Header("Broadcasts on")]
        [SerializeField] VoidEventChannelSO interactionReadyChannelSO;
        [SerializeField] VoidEventChannelSO interactionUnreadyChannelSO;
        [Header("Listens on")]
        [SerializeField] VoidEventChannelSO sceneReadyEventChannelSO;




        bool detectedPotentialInteractableLastUpdate = false;

        void OnEnable()
        {
            inputReader.InteractEvent += HandleInteractionInput;
            sceneReadyEventChannelSO.OnEventRaised += HandleSceneReadied;
        }

        void OnDisable()
        {
            inputReader.InteractEvent -= HandleInteractionInput;
            sceneReadyEventChannelSO.OnEventRaised -= HandleSceneReadied;
        }



        // Update is called once per frame
        void Update()
        {
            if(isInteractionEnabled)
                SetCurrentInteractable();
        }

        void SetCurrentInteractable()
        {
            if(interactablesRuntimeSet.runtimeSet.Count == 0)
                currentInteractable = null;
            else
                currentInteractable = interactablesRuntimeSet.runtimeSet[0];
            
            // Locate the closest interactable within range that is not behind the player
            Interactable nextCurrentInteractable = null;
            if(interactorTransformAnchor.Value != null)
                nextCurrentInteractable = FindNextCurrentInteractable(interactorTransformAnchor.Value, interactablesRuntimeSet.runtimeSet);
            

            // Clean up old highlight and draw new highlight
            currentInteractable?.SetHighlight(false);           // It's possible that the current interactable was destroyed last frame and thus would be missing ref
            nextCurrentInteractable?.SetHighlight(true);
            currentInteractable = nextCurrentInteractable;

            // If a change has been detected since last frame, raise the proper event
            bool detectedPotentialInteractableThisUpdate = (nextCurrentInteractable != null);
            if(detectedPotentialInteractableLastUpdate && !detectedPotentialInteractableThisUpdate)
            {
                interactionUnreadyChannelSO.RaiseEvent();
            }
            else if(!detectedPotentialInteractableLastUpdate && detectedPotentialInteractableThisUpdate)
            {
                interactionReadyChannelSO.RaiseEvent();
            }
            detectedPotentialInteractableLastUpdate = detectedPotentialInteractableThisUpdate;
        }

        Interactable FindNextCurrentInteractable(Transform interactorTransform, IEnumerable<Interactable> interactables)
        {
            Interactable nextCurrentInteractable = null;
            float smallestInteractableScore = float.MaxValue;
            
            foreach(Interactable interactable in interactables)
            {
                // Default turn off all interactables
                interactable.SetHighlight(false);

                Vector3 interactableVector = interactable.transform.position - interactorTransform.position;
                float interactableScore = Vector3.Dot(interactableVector, interactorTransform.forward);

                // We don't want to consider interactables that the player is not facing
                if(interactableScore < 0)
                    interactableScore = float.MaxValue;
                
                if(interactableScore < smallestInteractableScore)
                {
                    smallestInteractableScore = interactableScore;
                    nextCurrentInteractable = interactable;
                }
            }

            return nextCurrentInteractable;
        }


        void HandleInteractionInput()
        {
            if(currentInteractable != null)
            {
                currentInteractable.ExecuteInteraction();

                void CleanUpInteraction()
                {
                    // How can we clean up after having this be invoked?
                    //currentInteractable.OnInteractionFinished -= CleanUpInteraction;
                    isInteractionEnabled = true;
                    //inputReader.EnableGameplayInput();  // In case gameplay was disabled by interaction
                }

                currentInteractable.OnInteractionFinished += CleanUpInteraction;

                // Wipe the current interactable state
                currentInteractable.SetHighlight(false);
                currentInteractable = null;     

                // Prevent interaction manager from trying to select an interactable
                isInteractionEnabled = false;
            }
        }

        void HandleSceneReadied()
        {
            isInteractionEnabled = true;    // Just in case it was still false when the scene was last unloaded
        }

    }
}

