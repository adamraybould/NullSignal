using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects.Astro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI.World
{
    public class UI_EchoMarker : UI_StellarMarker
    {
        [Header("UI")] 
        [SerializeField] private Image m_echoBody;
        [SerializeField] private TextMeshProUGUI m_echoDistanceText;
        
        private EchoMarker m_echoMarker;
        private AudioSource m_audioSource;

        protected override void Awake()
        {
            base.Awake();
            
            m_audioSource = GetComponent<AudioSource>();
            m_echoMarker = m_stellarMarker as EchoMarker;
        }

        protected override void Start()
        {
            base.Start();
            m_echoMarker.OnDetected += OnDetected;
        }
        
        public override void Display()
        {
            base.Display();
            m_audioSource.Play();
        }

        private void OnDetected()
        {
            float distanceInLY = m_echoMarker.Echo.Source.Distance;
            float returnTime = distanceInLY / 10;
            
            // Set UI
            m_echoBody.color = m_echoMarker.Echo.Source.Color;
            m_echoDistanceText.text = "Distance: " + m_echoMarker.Echo.Source.Distance.ToString() + " LY";
            
            StartCoroutine(ActivateVisual(2));
        }

        private IEnumerator ActivateVisual(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Display();
        }
    }
}
