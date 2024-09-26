using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using Corruption.Core.Input;
using Corruption.Utility;
using UnityEngine;

namespace Corruption.Entities.Player.Components
{
    public class PlayerMovement : EntityMovement, IPlayerComponent
    {
        [Serializable]
        public struct MovementProperties
        {
            public float WalkSpeed;
            public float SprintSpeed;
            public float CrouchSpeed;
        }
        
        public enum MovementState
        {
            Walking,
            Sprinting,
            Crouching,
        }
        
        public Vector3 MovementDirection { get; private set; }
        public float MovementSpeed { get; private set; }
        public MovementState PlayerMovementState { get; private set; }
        
        [SerializeField] private MovementProperties m_movementProperties;
        [SerializeField] private float m_jumpStrength;
        
        [Space, SerializeField] private bool m_canMove = true;
        [SerializeField] private bool m_canJump = true;
        private bool m_hasJumped = false;

        [Header("Look Properties")] 
        [SerializeField] private PlayerCamera m_playerCamera;

        private AudioPlayer m_audioPlayer;
        
        protected override void Awake()
        {
            base.Awake();
            
            m_audioPlayer = GetComponent<AudioPlayer>();
        }

        private void Start()
        {
            SetUpInput();

            GameEvents.OnMenuOpened.Event += ctx => EnableControls(false);
            GameEvents.OnMenuClosed.Event += ctx => EnableControls(true);

            m_playerCamera.OnHeadBob += OnHeadBob;
        }

        protected override void Update()
        {
            base.Update();

            if (m_canMove)
            {
                ProcessMove();
            }

            if (m_hasJumped && IsGrounded)
            {
                m_hasJumped = false;
            }
        }

        private void SetUpInput()
        {
            PlayerController.Jump += OnJump;
            PlayerController.Sprint += Sprint;
            PlayerController.Crouch += Crouch;
        }

        private void ProcessMove()
        {
            Vector2 movementInput = PlayerController.MovementInput;
            if (movementInput != Vector2.zero)
            {
                MovementDirection = new Vector3(movementInput.x, 0.0f, movementInput.y);
                MovementSpeed = GetMovementSpeed();
                
                Vector3 worldMoveDir = m_playerCamera.GetCameraTransform().forward * MovementDirection.z + m_playerCamera.GetCameraTransform().right * MovementDirection.x;
                Move(worldMoveDir, MovementSpeed);
            }
            else
            {
                MovementDirection = Vector3.zero;
            }
        }

        private void OnJump()
        {
            if (m_canJump && !m_hasJumped)
            {
                Jump(m_jumpStrength);
                StartCoroutine(CheckIsGrounded());
            }
        }

        private void Sprint(bool value)
        {
            if (value)
            {
                PlayerMovementState = MovementState.Sprinting;
            }
            else
            {
                PlayerMovementState = MovementState.Walking;
            }
        }

        private void Crouch(bool value)
        {
            if (value)
            {
                PlayerMovementState = MovementState.Crouching;
            }
            else
            {
                PlayerMovementState = MovementState.Walking;
            }
        }

        private void OnHeadBob()
        {
            m_audioPlayer.Play();
        }
        
        private float GetMovementSpeed()
        {
            switch (PlayerMovementState)
            {
                case MovementState.Walking:
                {
                    return m_movementProperties.WalkSpeed;
                }
                
                case MovementState.Sprinting:
                {
                    return m_movementProperties.SprintSpeed;
                }

                case MovementState.Crouching:
                {
                    return m_movementProperties.CrouchSpeed;
                }

                default:
                {
                    return 0.0f;
                }
            }
        }

        private IEnumerator CheckIsGrounded()
        {
            yield return new WaitForSeconds(0.2f);
            m_hasJumped = true;
        }

        public void OnInteractiveFocusEnter()
        {
            m_canMove = false;
            m_canJump = false;
        }

        public void OnInteractiveFocusExit()
        {
            m_canMove = true;
            m_canJump = true;
        }
        
        public void EnableControls(bool value) { m_canMove = value; }
    }
}

