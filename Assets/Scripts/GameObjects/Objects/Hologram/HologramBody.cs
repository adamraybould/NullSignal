using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Objects
{
    public class HologramBody : MonoBehaviour
    {
        public Signal Signal { get; private set; }
        public StellarBody StellarBody { get; private set; }
        
        [SerializeField] private float m_orbitSpeed;
        [SerializeField] private CinemachineVirtualCamera m_selectionCamera;

        [Header("Visual Properties")] 
        [SerializeField] private Material m_discoveredMat;
        [SerializeField] private Material m_unknownMat;
        
        [Header("Glitching Properties")]
        [SerializeField] private float m_minGlitchDelay;
        [SerializeField] private float m_maxGlitchDelay;
        [SerializeField] private Vector2 m_glitchDurationMinMax;
        [SerializeField] private Vector2 m_glitchAmountMinMax;

        private Vector3 m_initialLocalPos;
        private float m_currentOrbitAngle;
        private float m_glitchTimer;

        private Renderer m_renderer;
        private Material m_material;

        private void Awake()
        {
            m_renderer = GetComponent<Renderer>();
            m_material = m_renderer.material;
        }

        private void Start()
        {
            m_initialLocalPos = transform.localPosition;
            m_glitchTimer = Random.Range(m_minGlitchDelay, m_maxGlitchDelay);
        }

        private void Update()
        {
            // Cause Glitching Effect
            if (m_maxGlitchDelay != 0)
            {
                m_glitchTimer -= Time.deltaTime;
                if (m_glitchTimer <= 0.0f)
                {
                    m_glitchTimer = Random.Range(m_minGlitchDelay, m_maxGlitchDelay);
                    StartCoroutine(Glitch());
                }
            }
            
            if (m_orbitSpeed != 0.0f)
            {
                if (m_currentOrbitAngle > 360.0f)
                    m_currentOrbitAngle = 0.0f;
                
                m_currentOrbitAngle += m_orbitSpeed * Time.deltaTime;
                float angleInRadians = m_currentOrbitAngle * Mathf.Deg2Rad;

                float x = Mathf.Cos(angleInRadians) * m_initialLocalPos.x - Mathf.Sin(angleInRadians) * m_initialLocalPos.z;
                float z = Mathf.Sin(angleInRadians) * m_initialLocalPos.x - Mathf.Cos(angleInRadians) * m_initialLocalPos.z;
                transform.localPosition = new Vector3(x, m_initialLocalPos.y, z);
            }
        }
        
        public void AttachSignal(Signal signal)
        {
            Signal = signal;
            
            Debug.Log("Discovery Percentage: " + signal.DiscoveryPercentage);
            Debug.Log("Discovered? - " + signal.Discovered);
            
            if (signal.Discovered)
            {
                m_renderer.material = m_discoveredMat;
                m_material = m_renderer.material;
            }
            else
            {
                m_renderer.material = m_unknownMat;
                m_material = m_renderer.material;
            }
        }
        
        public void SelectBody(bool value)
        {
            m_selectionCamera.enabled = value;
        }

        private IEnumerator Glitch()
        {
            float initialOrbitSpeed = m_orbitSpeed;
            m_orbitSpeed = 0.0f;
            m_material.SetFloat("_GlitchStrength", Random.Range(m_glitchAmountMinMax.x, m_glitchAmountMinMax.y));
            
            yield return new WaitForSeconds(Random.Range(m_glitchDurationMinMax.x, m_glitchDurationMinMax.y));

            m_orbitSpeed = initialOrbitSpeed;
            m_material.SetFloat("_GlitchStrength", 0.0f);
        }
    }
}
