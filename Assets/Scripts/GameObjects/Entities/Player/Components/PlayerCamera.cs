using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Entities.Player.Components
{
    public class PlayerCamera : MonoBehaviour, IPlayerComponent
    { 
        public event Action OnHeadBob;

        [SerializeField] private PlayerMovement m_playerMovement;
        [SerializeField] private CinemachineVirtualCamera m_virtualCamera;
        
        [Header("Head Bob")] 
        [SerializeField] private float m_headBobFrequency = 0.05f;
        [SerializeField] private float m_headBobSpeed = 3.0f;
        [SerializeField] private float m_headBobTransitionSpeed = 1.0f; 
        [SerializeField] private float m_headBobAmplitude = 0.5f; 
        private float m_headBobTimer = 0.0f;
        private bool m_hasTakenFootstep = false;
        
        [Header("Head Sway")] 
        [SerializeField] private float m_swayValue = 2.0f;
        [SerializeField] private float m_swaySpeed = 5.0f;

        private bool m_canLook = true;
        
        private Camera m_camera;
        private Vector3 m_initialCameraPosition;
        
        private Vector3 m_previousPosition; // Used for Interactive Focus
        private Quaternion m_previousRotation; // Used for Interactive Focus

        private void Awake()
        {
            m_camera = transform.Find("Camera").GetComponent<Camera>();
        }

        private void Start()
        {
            m_initialCameraPosition = m_virtualCamera.transform.localPosition;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (m_canLook)
            {
                HeadBob(m_playerMovement.MovementDirection);
                HeadSway(m_playerMovement.MovementDirection);
            }
        }

        private void HeadBob(Vector3 inputDirection)
        {
            if (inputDirection != Vector3.zero)
            {
                Vector3 headBobPosition = m_initialCameraPosition;
                
                if (m_playerMovement.MovementDirection.magnitude > 0.1f)
                {
                    m_headBobTimer += (m_headBobSpeed * m_playerMovement.MovementSpeed) * Time.deltaTime;
                    float offsetY = Mathf.Sin(m_headBobTimer) * m_headBobFrequency;
                    float offsetZ = Mathf.Cos(m_headBobTimer) * m_headBobFrequency * m_headBobAmplitude;
                    
                    headBobPosition.y += offsetY;
                    headBobPosition.z += offsetZ;

                    CheckforHeadBob(headBobPosition.y);
                }
                
                m_virtualCamera.transform.localPosition = Vector3.Lerp(m_virtualCamera.transform.localPosition, headBobPosition, m_headBobTransitionSpeed * Time.deltaTime);
            }
            else
            {
                m_headBobTimer = 0.0f;
                m_virtualCamera.transform.localPosition = Vector3.Lerp(m_virtualCamera.transform.localPosition, m_initialCameraPosition, m_headBobTransitionSpeed * Time.deltaTime);
            }
        }
        
        private void HeadSway(Vector3 inputDirection)
        {
            if (inputDirection != Vector3.zero)
            {
                m_virtualCamera.m_Lens.Dutch = Mathf.Lerp(m_virtualCamera.m_Lens.Dutch, m_swayValue * -inputDirection.x, m_swaySpeed * Time.deltaTime);
            }
            else
            {
                m_virtualCamera.m_Lens.Dutch = Mathf.Lerp(m_virtualCamera.m_Lens.Dutch, 0.0f, m_swaySpeed * Time.deltaTime);
            }
        }

        // -- Used for the Head Bob SFX
        private void CheckforHeadBob(float offset)
        {
            if (offset < 0.0f && !m_hasTakenFootstep)
            {
                OnHeadBob?.Invoke();
                m_hasTakenFootstep = true;
            }
            else if (offset > 0.0f && m_hasTakenFootstep)
            {
                m_hasTakenFootstep = false;
            }
        }
        
        public void OnInteractiveFocusEnter()
        {
            m_canLook = false;
        }

        public void OnInteractiveFocusExit()
        {
            m_canLook = true;
        }
        
        public Transform GetCameraTransform() { return m_camera.transform; }
    }
}
