using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class EchoRadar : MonoBehaviour
    {
        public event Action<float> OnPulse;
        public event Action OnPulseReset;
        
        public event Action<Echo> OnEchoScanBegin;

        public bool Pulsed { get; private set; }
        public bool CanPulse { get; private set; }
        public float PulseCooldown => m_pulseCooldown;
        public float PulseSpeed => m_pulseSpeed;
        public float PulseRange => m_pulseRange;

        public List<Echo> DetectedEchoes => m_detectedEchoes;
        
        [Header("Pulse")] 
        [SerializeField] private float m_pulseRange;
        [SerializeField] private float m_pulseSpeed;
        [SerializeField] private float m_pulseCooldown;
        
        private float m_pulseReachedDistance;
        private SphereCollider m_pulseDetectionCollider;
        
        [Header("Echoes")]
        [SerializeField] private List<Echo> m_detectedEchoes;

        private void Awake()
        {
            Pulsed = false;
            CanPulse = true;
            
            m_pulseDetectionCollider = GetComponent<SphereCollider>();
            m_pulseDetectionCollider.radius = 0.5f;
        }

        private void Update()
        {
            if (Pulsed)
            { 
                ScanForEchoes();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            EchoMarker echoMarker = other.gameObject.GetComponent<EchoMarker>();
            if (echoMarker && !m_detectedEchoes.Contains(echoMarker.Echo))
            {
                echoMarker.Detect();
                DetectedEchoes.Add(echoMarker.Echo);
            }
        }
        
        public bool Pulse()
        {
            if (!CanPulse || Pulsed)
                return false;

            CanPulse = false;
            Pulsed = true;

            StartCoroutine(RechargePulse());
            OnPulse?.Invoke(m_pulseRange);
            return true;
        }

        public bool TriangulateEcho(Echo detectedEcho)
        {
            
            
            OnEchoScanBegin?.Invoke(detectedEcho);
            return true;
        }

        public bool HasEchoBeenDiscovered(Echo echo)
        {
            foreach (Echo detectedEcho in DetectedEchoes)
            {
                if (detectedEcho == echo)
                    return true;
            }

            return false;
        }

        private void ScanForEchoes()
        {
            // Expand Sphere Collider to detect Echoes
            m_pulseDetectionCollider.radius += (m_pulseSpeed * 100.0f) * Time.deltaTime;
            m_pulseReachedDistance += Time.deltaTime;

            if (m_pulseReachedDistance >= m_pulseRange)
            {
                ResetPulse();
            }
        }

        private void ResetPulse()
        {
            Pulsed = false;
            m_pulseDetectionCollider.radius = 0.0f;
            m_pulseReachedDistance = 0.0f;
            
            OnPulseReset?.Invoke();
        }

        private IEnumerator RechargePulse()
        {
            yield return new WaitForSeconds(PulseCooldown);
            CanPulse = true;
        }
    }
}
