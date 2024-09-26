using System;
using System.Collections;
using Corruption.Astro;
using Corruption.Core;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects
{
    public class RadarSatellite : MonoBehaviour
    {
        public event Action<Echo, float> OnDetectionBegin;
        public event Action<Signal> OnDetectionComplete;
        public event Action OnDetectionCancelled;
        
        public bool IsScanning { get; private set; }

        [Header("Detection Properties")] 
        [SerializeField] private RadarDetectionPoint m_detectionPoint;
        [SerializeField] private float m_detectionRange;
        [SerializeField] private float m_scanDuration; // The Amount of time it takes to fully scan a signal
        [SerializeField] private float m_scanDurationReduction;
        
        private bool m_canPassiveScan = true;
        private RadarSatelliteController m_satelliteController;

        private void Awake()
        {
            m_satelliteController = GetComponent<RadarSatelliteController>();
        }

        private void Start()
        {
            PlayerController.Input.Radar.Scan.performed += ctx => AttemptScan();
            PlayerController.Input.Radar.Scan.canceled += ctx => CancelSignalScan();
        }

        private void Update()
        {
            // Is being Controlled by the Player
            if (m_satelliteController.IsControlled)
            {
                PassiveScan(); // Passively scans for Signals
            }
        }

        public void AttemptScan()
        {
            m_satelliteController.IsControlled = false;
            IsScanning = true;
            m_canPassiveScan = false;
            
            StartCoroutine(ScanSignal());
        }
        
        private void PassiveScan()
        {
            if (!m_canPassiveScan)
                return;
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, 1 << 8))
            {
                m_detectionPoint.transform.position = hit.point;
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            }
            
            m_detectionPoint.DetectSignals();
            if (m_detectionPoint.SignalPosition != Vector3.zero)
            {
                Debug.DrawLine(m_detectionPoint.transform.position, m_detectionPoint.SignalPosition, Color.cyan);
            }
        }

        private IEnumerator ScanSignal()
        {
            float scanDuration = m_scanDuration;
            
            Echo echo = m_detectionPoint.GetClosestSignal(m_detectionRange);
            if (echo != null)
            {
                scanDuration = (m_scanDuration * echo.GetDifficultyMultiplier()) - m_scanDurationReduction;
                scanDuration = Mathf.Clamp(scanDuration, 0.0f, 100.0f);
            }

            OnDetectionBegin?.Invoke(echo, scanDuration);
            yield return new WaitForSeconds(scanDuration);
            
            Debug.Log("Scan Complete");
            Signal signal = new Signal(echo);
            OnDetectionComplete?.Invoke(signal);

            m_satelliteController.IsControlled = true;
            IsScanning = false;
            m_canPassiveScan = true;
        }
        
        private void CancelSignalScan()
        {
            if (IsScanning)
            {
                Debug.Log("Scan Cancelled");
                
                StopAllCoroutines();
                OnDetectionCancelled?.Invoke();

                m_satelliteController.IsControlled = true;
                IsScanning = false;
                m_canPassiveScan = true;
            }
        }
    }
}
