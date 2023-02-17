using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class TestInput : MonoBehaviour
    {
        PlayerInput inputActions;
        [SerializeField] Transform player;


        private void Awake()
        {
            inputActions = new PlayerInput();
            inputActions.Player.Enable();

        }
        private void Update()
        {

        }

    }
}
