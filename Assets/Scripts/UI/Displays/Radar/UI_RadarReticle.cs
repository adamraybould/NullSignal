using System;
using System.Collections;
using Corruption.Astro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.UI.Display
{
    public class UI_RadarReticle : MonoBehaviour
    {
        [SerializeField] private UI_RadarDisplay m_radarDisplay;

        [SerializeField] private Transform m_reticleOuter;
        [SerializeField] private int m_minRandomSpinDelay;
        [SerializeField] private int m_maxRandomSpinDelay;
        
        private bool m_isActive;
        private Vector2 m_signalPosition;
        
        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_radarDisplay.Monitor.OnFocusEnter += OnFocusEnter;
            m_radarDisplay.Monitor.OnFocusExit += OnFocusExit;
            m_radarDisplay.Satellite.OnDetectionBegin += OnDetectionBegin;
            m_radarDisplay.Satellite.OnDetectionCancelled += OnDetectionCancelled;
            m_radarDisplay.Satellite.OnDetectionComplete += ctx => OnScanComplete();
        }

        private void Update()
        {
            /*
            if (!m_centered)
            {
                transform.localPosition = Vector2.Lerp(transform.localPosition, transform.TransformPoint(m_signalPosition), Time.deltaTime);
                if (Vector2.Distance(transform.position, m_signalPosition) <= 1.0f)
                    m_centered = true;
            }
            */
        }

        private void OnFocusEnter()
        {
            m_isActive = true;
            StartCoroutine(RandomReticleSpin());
        }

        private void OnFocusExit()
        {
            m_isActive = false;
            StopAllCoroutines();
        }

        private void OnDetectionBegin(Echo echo, float scanDuration)
        {
            m_animator.SetFloat("Scan_Speed", 1.0f / scanDuration);
            m_animator.SetBool("Scanning", true);
            m_animator.SetTrigger("Scan");
        }

        private void OnDetectionCancelled()
        {
            m_animator.SetTrigger("Scan_Cancelled");
            m_animator.SetBool("Scanning", false);
        }

        private void OnScanComplete()
        {
            m_animator.SetTrigger("Scan_Complete");
            m_animator.SetBool("Scanning", false);
        }

        IEnumerator RandomReticleSpin()
        {
            while (true)
            {
                m_animator.SetTrigger("Spin");
                yield return new WaitForSeconds(1.0f + Random.Range(m_minRandomSpinDelay, m_maxRandomSpinDelay));
            }
        }
    }
}
