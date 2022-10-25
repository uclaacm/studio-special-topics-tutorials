using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StudioLand
{
    [CreateAssetMenu( fileName = "InputReader", menuName = "ScriptableObjects/Input Reader")]
    public class InputReaderSO : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IPersistentActions
    {
        GameInput gameInput = default;
        [SerializeField] InputActionAsset uiInput;
       
        // Callbacks
        public event System.Action<Vector2> MoveEvent = delegate {};
        public event System.Action<Vector2> RotateCameraEvent = delegate {};
        public event System.Action InteractEvent = delegate {};
        public event System.Action QuitEvent = delegate {};


        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRotateCamera(InputAction.CallbackContext context)
        {
            RotateCameraEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractEvent?.Invoke();
        }

        public void OnQuit(InputAction.CallbackContext context)
        {
            if(context.performed)
                QuitEvent?.Invoke();
        }

        private InputActionReference cameraRotation;
        public InputActionReference GetCameraRotationActionReference => cameraRotation; // Method for injecting reference to external input modules

        // NOTE: OnEnable for a ScriptableObject is called either when it is first created or the project is recompiled
        void OnEnable()
        {
            // NOTE: This whole ScriptableObject is simply a wrapper around a single GameInput instance
            if(gameInput == null)
            {
                // Because the project always recompiles when the input mapping changes, this SO will always be up to date
                gameInput = new GameInput();
                gameInput.Gameplay.SetCallbacks(this);
                gameInput.Persistent.SetCallbacks(this);
                gameInput.Persistent.Enable();      // Keep this enabled throughout the entire duration of the game
                
                cameraRotation = InputActionReference.Create(gameInput.Gameplay.RotateCamera);
            }
        }

        public void EnableGameplayInput()
        {
            gameInput.Gameplay.Enable();
            uiInput.Disable();
        }

        public void EnableUIInput()
        {
            gameInput.Gameplay.Disable();
            uiInput.Enable();
        }

        public void DisableAllInput()
        {
            gameInput.Gameplay.Disable();
            uiInput.Disable();
        }
    }
}

