using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using Corruption.Objects;
using TMPro;
using UnityEngine;

namespace Corruption.UI
{
    public class UIHologram_StarDetails : UIHologram_Panel
    {
        [Header("Star Details")]
        [SerializeField] private TextMeshProUGUI m_starNameText;
        [SerializeField] private TextMeshProUGUI m_starTypeText;
        
        [SerializeField] private TextMeshProUGUI m_starClassText;
        [SerializeField] private TextMeshProUGUI m_starSolarMassText;

        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public override void SetHologramBody(HologramBody body)
        {
            base.SetHologramBody(body);
            
            Star star = (Star)body.StellarBody;
            
            m_starNameText.text = body.Signal.GetBodyName();
            m_starTypeText.text = body.Signal.GetBodyType();

            m_starClassText.text = star.Classification.ToString();
            m_starSolarMassText.text = star.SolarMass.ToString("0.0");
        }

        public void Close()
        {
            m_animator.SetTrigger("Close");
        }

        public void Anim_OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}
