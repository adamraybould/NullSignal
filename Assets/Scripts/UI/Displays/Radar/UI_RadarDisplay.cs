using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI.Display
{
    public class UI_RadarDisplay : UIDisplay
    {
        public RadarSatellite Satellite => m_satellite;
        public RadarSatelliteController SatelliteController => m_satelliteController;

        [SerializeField] private RadarSatelliteController m_satelliteController;
        [SerializeField] private RadarSatellite m_satellite;

        [Header("UI")] 
        [SerializeField] private Image m_reticle;
        [SerializeField] private Image m_pingCircle;
        [SerializeField] private float m_pingCircleSpeed;
        [SerializeField] private float m_pingCircleFadeSpeed;
            
        [Space, SerializeField] private TextMeshProUGUI m_cooldownText;
        [SerializeField] private TextMeshProUGUI m_longitudeText;
        [SerializeField] private TextMeshProUGUI m_latitudeText;

        private float m_longitude;
        private float m_latitude;

        protected override void Awake()
        {
            base.Awake();
            
            m_longitude = 0.0f;
            m_latitude = 0.0f;
        }

        protected override void Start()
        {
            base.Start();

            m_satelliteController.OnRotate += OnSatelliteRotate;
            //m_satellite.OnPing += OnSignalPing;
            m_satellite.OnDetectionBegin += OnDetectionBegin;
            m_satellite.OnDetectionCancelled += OnDetectionCancelled;
            m_satellite.OnDetectionComplete += OnDetectionComplete;
        }

        private void Update()
        {
            m_longitudeText.text = "Longitude: " + m_longitude.ToString("0.0");
            m_latitudeText.text = "Latitude: " + m_latitude.ToString("0.0");
        }

        protected override void OnFocusEnter()
        {
            base.OnFocusEnter();
            m_satelliteController.IsControlled = true;
        }

        protected override void OnFocusExit()
        {
            base.OnFocusExit();
            m_satelliteController.IsControlled = false;
        }

        private void OnSatelliteRotate(Vector2 rotation)
        {
            m_longitude = -rotation.x;
            m_latitude = -rotation.y;
        }

        private void OnDetectionBegin(Echo echo, float scanDuration)
        {
            PlayAudio(1, true);
        }

        private void OnDetectionCancelled()
        {
            PlayAudio(3);
        }
        
        private void OnDetectionComplete(Signal signal)
        {
            if (signal != null)
                PlayAudio(2);
            else
                PlayAudio(3);
        }

        private void ScalePingCircle()
        {
            float newScale = m_pingCircle.transform.localScale.x + (m_pingCircleSpeed * Time.deltaTime);
            m_pingCircle.transform.localScale = new Vector3(newScale, newScale, newScale);

            Color circleColor = m_pingCircle.color;
            circleColor.a = Mathf.Lerp(circleColor.a, 0.0f, m_pingCircleFadeSpeed * Time.deltaTime);
            m_pingCircle.color = circleColor;
        }
    }
}
