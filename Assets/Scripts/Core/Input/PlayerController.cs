using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Core.Input
{
    public class PlayerController : MonoBehaviour
    {
        public static event Action Jump;
        public static event Action<bool> Sprint;
        public static event Action<bool> Crouch;
        
        public static event Action Interact;
        public static event Action ExitFocus;
        
        
        public static Vector2 MovementInput => Input.PlayerCharacter.Movement.ReadValue<Vector2>();
        public static Vector2 MouseInput => Input.PlayerCharacter.Look.ReadValue<Vector2>();
        
        public static Controls Input { get; private set; }

        private void Awake()
        {
            SetUpInput();
        }

        private void OnEnable() { Input.Enable(); }
        private void OnDisable() { Input.Disable(); }
        
        private static void SetUpInput()
        {
            Input = new Controls();
            Input.Enable();

            Input.PlayerCharacter.Jump.performed += ctx => Jump?.Invoke();

            Input.PlayerCharacter.Sprint.performed += ctx => Sprint?.Invoke(true);
            Input.PlayerCharacter.Sprint.canceled += ctx => Sprint?.Invoke(false);

            Input.PlayerCharacter.Crouch.performed += ctx => Crouch?.Invoke(true);
            Input.PlayerCharacter.Crouch.canceled += ctx => Crouch?.Invoke(false);

            Input.PlayerInteraction.Interact.performed += ctx => Interact?.Invoke();
            Input.PlayerInteraction.ExitFocus.performed += ctx => ExitFocus?.Invoke();
        }
    }
}
