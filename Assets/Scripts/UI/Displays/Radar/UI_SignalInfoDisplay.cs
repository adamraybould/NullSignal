using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using TMPro;
using UnityEngine;

namespace Corruption.UI.Display
{
    public class UI_SignalInfoDisplay : UIDisplay
    {
        [SerializeField] private RadarSatellite m_satellite;

        [Header("UI")] 
        [SerializeField] private TextMeshProUGUI m_signalStatusText;
        [SerializeField] private TextMeshProUGUI m_signalSourceText;
        [SerializeField] private TextMeshProUGUI m_signalStellarClassifcationText;

        [Space] 
        [SerializeField] private float m_idleTextUpdateDelay;
        
        protected override void Start()
        {
            base.Start();

            StartCoroutine(IdleTextUpdate());
            m_satellite.OnDetectionBegin += OnDetectionBegin;
            m_satellite.OnDetectionComplete += OnDetectionComplete;
        }
        
        IEnumerator IdleTextUpdate()
        {
            while (true)
            {
                m_signalSourceText.text = "";
                m_signalStellarClassifcationText.text = "";
                yield return new WaitForSeconds(m_idleTextUpdateDelay);
                m_signalSourceText.text = ".";
                m_signalStellarClassifcationText.text = ".";
                yield return new WaitForSeconds(m_idleTextUpdateDelay);
                m_signalSourceText.text = "..";
                m_signalStellarClassifcationText.text = "..";
                yield return new WaitForSeconds(m_idleTextUpdateDelay);
                m_signalSourceText.text = "...";
                m_signalStellarClassifcationText.text = "...";
                yield return new WaitForSeconds(m_idleTextUpdateDelay);
            }
        }

        private void OnDetectionBegin(Echo echo, float scanDuration)
        {
            m_signalStatusText.text = "Scanning Signal";
            
            StopAllCoroutines(); 
            StartCoroutine(IdleTextUpdate());
        }

        private void OnDetectionComplete(Signal signal)
        {
            StopAllCoroutines();

            if (signal != null)
            {
                m_signalStatusText.text = "Signal Detected";
                //m_signalSourceText.text = signal.StellarBody.Source.ToString();
                m_signalStellarClassifcationText.text = signal.StellarBody.Type.ToString();
            }
            else
            {
                m_signalStatusText.text = "No Signal Detected";
            }
        }
    }
}
