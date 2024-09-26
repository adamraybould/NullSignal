using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects;
using Corruption.Objects.Astro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI.Display
{
    public class UI_EchoRadarDisplay : UIDisplay
    {
        [SerializeField] private EchoRadar m_radar;

        [Header("UI")] 
        [SerializeField] private Transform m_radarReticle;
        [SerializeField] private RawImage m_grid;
        [SerializeField] private float m_rippleEffectLength;

        private Material m_rippleMaterial;
        private bool m_pinged;
        private float m_pingMaterialTimer;

        protected override void Awake()
        {
            base.Awake();
            
            m_rippleMaterial = m_grid.material;
        }

        protected override void Start()
        {
            base.Start();

            //m_radar.OnPing += TriggerRippleEffect;
        }

        private void Update()
        {
            if (m_pinged)
            {
                m_pingMaterialTimer += Time.deltaTime;
                m_rippleMaterial.SetFloat("_Timer", m_pingMaterialTimer);
            }
        }

        private void TriggerRippleEffect()
        {
            StartCoroutine(RippleEffect());
            PlayAudio(1);
        }

        private IEnumerator RippleEffect()
        {
            m_rippleMaterial.SetFloat("_Size", 0.1f);
            m_pinged = true;
                
            yield return new WaitForSeconds(m_rippleEffectLength);

            m_pinged = false;
            m_rippleMaterial.SetFloat("_Size", 0.0f);
            m_pingMaterialTimer = 0.0f;
        }
    }
}
