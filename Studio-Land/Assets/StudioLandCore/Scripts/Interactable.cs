using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StudioLand
{
    /// <summary>
    /// Class for anything that can be interacted with in the game. Displays requested interaction input to user based on
    /// a trigger and invokes a UnityEvent when it detects that press (just like a button but you need to be in the
    /// range of the interactable)
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] GameObject interactIndicator;
        [SerializeField] InteractablesRuntimeSetSO interactablesRuntimeSet;

        public event System.Action OnInteractionFinished;

        bool isWithinRange = false;

        /// <summary> Toggle the visual indicator for this interactable </summary>
        public void SetHighlight(bool shouldHighlight)
        {
            interactIndicator.SetActive(shouldHighlight);
        }

        /// <summary> Implement the interaction </summary>
        public abstract void ExecuteInteraction();

        /// <summary> Function for notifying listeners that the interaction has been completed </summary>
        protected void FinishInteraction()
        {
            // MESSAGE PASSING TRADEOFF NOTES:

            // The reason we do an event call and not a direct call to the InteractionManager is because we don't want
            // an unnecessary dependency with the InteractionManager.
            // While it is true that the IM is now forced to have a dependency with the interactable to subscribe to
            // its event, the IM naturally has to have a dependency with Interactables since it is literally an
            // "Interactable" manager, and thus another dependency doesn't really change much.

            // Another option for communicating finished status would have been to pass a message through a dedicated
            // channel like the "InteractionFinishedEventChannel" or something like that. However, that's another
            // ScriptableObject we have to take care of and remember to put into the editor. It would only be worth
            // if the IM didn't already have a dependency with Interactables and thus would be significantly more
            // coupled if it was forced to subscribe to an event from Interactable.

            OnInteractionFinished?.Invoke();
        }

        void OnEnable()
        {
            // Default state is off and can be switched on if player detected
            interactIndicator.SetActive(false);
        }

        void OnDisable()
        {
            // In case it the gameobject is deactivated without the player leaving the trigger zone
            interactablesRuntimeSet.runtimeSet.Remove(this);
        }


        void OnTriggerEnter(Collider collider)
        {
            // This is a relatively simple project so tag checking should be fine as opposed to a more robust method
            if(collider.CompareTag("Player"))
            {
                interactablesRuntimeSet.runtimeSet.Add(this);
            }
                
        }

        void OnTriggerExit(Collider collider)
        {
            if(collider.CompareTag("Player"))
            {
                interactablesRuntimeSet.runtimeSet.Remove(this);
            }      
        }
    }
}

