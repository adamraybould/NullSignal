using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Corruption.Core.Framework;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class GalaxyMapCamera : MonoBehaviour
    {
        public Camera Camera => m_camera;
        
        [SerializeField] private Camera m_camera;
        [SerializeField] private Transform m_vCameraPivot;
        [SerializeField] private CinemachineVirtualCamera m_vCamera;
        private Cinemachine3rdPersonFollow m_vCameraFollow;

        [Header("Movement")]
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_sensitivity;
        
        [Space, SerializeField] private MinMaxRange m_minMaxZoom;
        [SerializeField] private float m_zoomSpeed;
        private float m_zoomLevel;

        private Transform m_focusPoint;
        
        private bool m_canRotate;
        private bool m_canZoom = true;

        private void Awake()
        {
            m_vCameraFollow = m_vCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            m_zoomLevel = m_vCameraFollow.VerticalArmLength;
        }

        private void Start()
        {
            SetUpInput();
        }

        private void Update()
        {
            // Track Follow Point if Valid
            if (m_focusPoint)
            {
                m_vCameraPivot.transform.position = m_focusPoint.position;
            }
            
            if (m_canZoom)
            {
                Vector2 zoomInput = PlayerController.Input.GalaxyMap.Zoom.ReadValue<Vector2>();
                ProcessZoom(zoomInput);
            }

            if (m_canRotate)
            {
                Vector2 mouseMovement = PlayerController.Input.GalaxyMap.MouseDelta.ReadValue<Vector2>();
                if (mouseMovement != Vector2.zero)
                {
                    RotateCameraAroundCentre(mouseMovement * (m_sensitivity * Time.deltaTime));
                }
            }
        }
        
        public void SetCameraActive(bool value) 
        { 
            gameObject.SetActive(value);
        }

        public void SetCameraFocus(Transform focusPoint)
        {
            m_focusPoint = focusPoint;
            m_vCameraPivot.transform.position = m_focusPoint.position;
            m_canZoom = false;
        }

        public void ResetCameraFocus()
        {
            m_focusPoint = null;
            m_vCameraPivot.transform.localPosition = Vector3.zero;
            m_canZoom = true;
        }

        private void ProcessMovement(Vector2 input)
        {
            /*
            if (input != Vector2.zero)
            {
                Vector3 movementDirection = new Vector3(input.x, 0.0f, input.y);
                Vector3 worldMoveDirection = m_cameraPivot.transform.forward * movementDirection.z + m_cameraPivot.right * movementDirection.x;
                worldMoveDirection.y = 0.0f;
                
                transform.position += worldMoveDirection * (m_movementSpeed * Time.deltaTime);
            }
            */
        }

        private void ProcessZoom(Vector2 input)
        {
            if (input != Vector2.zero)
            {
                /*
                Vector3 forwardDirection = (m_vCamera.transform.forward * input.y).normalized;
                Vector3 newPosition = m_vCamera.transform.position + forwardDirection * (m_zoomSpeed * Time.deltaTime);
                
                float distanceToCenter = Vector3.Distance(newPosition, transform.position);
                float clampedDistance = Mathf.Clamp(distanceToCenter, m_minMaxZoom.Min, m_minMaxZoom.Max);
                if (distanceToCenter >= m_minMaxZoom.Min && distanceToCenter <= m_minMaxZoom.Max)
                {
                    //m_vCamera.transform.position = newPosition;
                }
                */
                
                m_zoomLevel += (m_zoomSpeed * Time.deltaTime) * -input.y;
                m_zoomLevel = Mathf.Clamp(m_zoomLevel, m_minMaxZoom.Min, m_minMaxZoom.Max);
                
                m_vCameraFollow.VerticalArmLength = m_zoomLevel;
                m_vCameraFollow.CameraDistance = m_zoomLevel;
            }
        }

        private void RotateCameraAroundCentre(Vector2 input)
        {
            Quaternion newRotation = m_vCameraPivot.transform.localRotation * Quaternion.Euler(-input.y, input.x, 0);
            Vector3 eular = newRotation.eulerAngles;
            newRotation = Quaternion.Euler(eular.x, eular.y, 0.0f);
            
            m_vCameraPivot.transform.localRotation = newRotation;
        }

        private void SetUpInput()
        {
            PlayerController.Input.GalaxyMap.Rotate.performed += ctx => { m_canRotate = true; };
            PlayerController.Input.GalaxyMap.Rotate.canceled += ctx => { m_canRotate = false; };
        }
        
        public Quaternion GetCameraRotation() { return m_vCameraPivot.transform.localRotation; }
    }
}
