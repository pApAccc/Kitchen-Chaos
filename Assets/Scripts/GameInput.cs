using Common.SavingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameInput : MonoBehaviour, ISaveable
    {
        public event EventHandler OnInteract;
        public event EventHandler OnCutting;
        public event EventHandler OnPause;


        public static GameInput Instance { get; private set; }
        private PlayerInput inputActions;
        private string inputSaveData;


        public enum Binding
        {
            Move_Up,
            Move_Down,
            Move_Left,
            Move_Right,
            Interact,
            InteractAlternate,
            Pause,
        }


        private void Awake()
        {
            Instance = this;
            inputActions = new PlayerInput();

            inputActions.Enable();
            inputActions.Player.Interact.performed += Interact_performed;
            inputActions.Player.InteractAlternate.performed += Cutting_performed;
            inputActions.Player.Pause.performed += Pause_performed;
        }


        private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }

        private void Cutting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnCutting?.Invoke(this, EventArgs.Empty);
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMoveDirInput()
        {
            Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            return moveInput.normalized;
        }

        private void OnDestroy()
        {
            inputActions.Player.Interact.performed -= Interact_performed;
            inputActions.Player.InteractAlternate.performed -= Cutting_performed;
            inputActions.Player.Pause.performed -= Pause_performed;
            inputActions.Dispose();
        }

        public string GetBindingKey(Binding binding)
        {
            switch (binding)
            {
                case Binding.Interact:
                    return inputActions.Player.Interact.bindings[0].ToDisplayString();
                case Binding.InteractAlternate:
                    return inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
                case Binding.Pause:
                    return inputActions.Player.Pause.bindings[0].ToDisplayString();
                case Binding.Move_Up:
                    return inputActions.Player.Move.bindings[1].ToDisplayString();
                case Binding.Move_Down:
                    return inputActions.Player.Move.bindings[2].ToDisplayString();
                case Binding.Move_Left:
                    return inputActions.Player.Move.bindings[3].ToDisplayString();
                case Binding.Move_Right:
                    return inputActions.Player.Move.bindings[4].ToDisplayString();
                default:
                    return null;
            }

        }

        public void ReBindBinding(Binding binding, Action BindingCompleteCallback)
        {
            inputActions.Player.Disable();
            InputAction inputAction = null;
            int index = 0;
            switch (binding)
            {
                case Binding.Interact:
                    inputAction = inputActions.Player.Interact;
                    index = 0;
                    break;
                case Binding.InteractAlternate:
                    inputAction = inputActions.Player.InteractAlternate;
                    index = 0;
                    break;
                case Binding.Pause:
                    inputAction = inputActions.Player.Pause;
                    index = 0;
                    break;
                case Binding.Move_Up:
                    inputAction = inputActions.Player.Move;
                    index = 1;
                    break;
                case Binding.Move_Down:
                    inputAction = inputActions.Player.Move;
                    index = 2;
                    break;
                case Binding.Move_Left:
                    inputAction = inputActions.Player.Move;
                    index = 3;
                    break;
                case Binding.Move_Right:
                    inputAction = inputActions.Player.Move;
                    index = 4;
                    break;
                default: Debug.LogError("找不到要绑定的按键"); break;
            }
            inputAction.PerformInteractiveRebinding(index).OnComplete((callback) =>
            {
                callback.Dispose();
                inputActions.Player.Enable();
                BindingCompleteCallback();

                inputSaveData = inputAction.SaveBindingOverridesAsJson();
            }).Start();

        }

        public object CaptureState()
        {
            return inputSaveData;
        }

        public void RestoreState(object state)
        {
            inputActions.Disable();
            if (!string.IsNullOrEmpty((string)state))
            {
                inputActions.LoadBindingOverridesFromJson((string)state);
            }
            inputActions.Enable();
        }
    }
}
