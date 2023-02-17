using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GameInput : MonoBehaviour
    {
        public event EventHandler OnInteract;
        public event EventHandler OnCutting;

        public static GameInput Instance { get; private set; }
        private PlayerInput inputActions;
        private void Awake()
        {
            Instance = this;

            inputActions = new PlayerInput();
            inputActions.Enable();

            inputActions.Player.Interact.performed += Interact_performed;
            inputActions.Player.Cutting.performed += Cutting_performed;
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


    }
}
