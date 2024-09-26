using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects
{
    public class RadarSatelliteController : MonoBehaviour
    {
        public event Action<Vector2> OnRotate; 
        
        public bool IsControlled { get; set; }
        
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_minMaxXRotation;
        [SerializeField] private float m_minMaxYRotation;
        
        [Space, SerializeField] private RadarDetectionPoint m_detectionPoint;
        [SerializeField] private float m_centerSpeed; // The Speed that the satellite centers on a signal
        
        private float m_xRotation = 0.0f;
        private float m_yRotation = 0.0f;

        private RadarSatellite m_radar;

        private void Awake()
        {
            m_radar = GetComponent<RadarSatellite>();
        }

        private void Start()
        {
            m_radar.OnDetectionBegin += OnDetectionBegin;
        }

        private void Update()
        {
            // Is being Controlled by the Player
            if (IsControlled)
            {
                ProcessMovement(PlayerController.Input.Radar.Movement.ReadValue<Vector2>());
            }
        }
        
        private void ProcessMovement(Vector2 input)
        {
            if (input != Vector2.zero)
            {
                Vector2 movement = input * (m_movementSpeed * Time.deltaTime);
                
                m_xRotation += movement.y;
                m_yRotation += movement.x; 
                m_xRotation = Mathf.Clamp(m_xRotation, -m_minMaxYRotation, m_minMaxYRotation);
                m_yRotation = Mathf.Clamp(m_yRotation, -m_minMaxXRotation, m_minMaxXRotation);
                transform.localRotation = Quaternion.Euler(-m_xRotation, m_yRotation, 0.0f);

                OnRotate?.Invoke(new Vector2(m_xRotation, m_yRotation));
            }
        }

        private void OnDetectionBegin(Echo echo, float scanDuration)
        {
            StartCoroutine(CenterOnSignal(echo));
        }
        
        private IEnumerator CenterOnSignal(Echo echo)
        {
            if (echo != null)
            {
                Vector3 direction = m_detectionPoint.SignalPosition - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                
                while (m_radar.IsScanning)
                {
                    float deltaXRotation = (-(targetRotation.eulerAngles.x - transform.eulerAngles.x)) * m_centerSpeed * Time.deltaTime;
                    float deltaYRotation = (targetRotation.eulerAngles.y - transform.eulerAngles.y) * m_centerSpeed * Time.deltaTime;
                    
                    Vector2 interpolatedRotation = new Vector2(deltaYRotation, deltaXRotation);
                    ProcessMovement(interpolatedRotation);
                    yield return null;
                }
            }
            
            yield return null;
        }
    }
}
