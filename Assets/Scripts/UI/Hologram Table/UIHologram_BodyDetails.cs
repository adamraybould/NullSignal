using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using TMPro;
using UnityEngine;

namespace Corruption.UI
{
    public class UIHologram_BodyDetails : UIHologram_Panel
    {
        [Header("Body Details")]
        [SerializeField] private TextMeshProUGUI m_bodyNameText;
        [SerializeField] private TextMeshProUGUI m_bodyTypeText;
        
        [SerializeField] private TextMeshProUGUI m_bodyMassText;
        [SerializeField] private TextMeshProUGUI m_bodyGravityText;
        [SerializeField] private TextMeshProUGUI m_bodySurfaceTempText;
        [SerializeField] private TextMeshProUGUI m_bodyAtmosphereText;

        [SerializeField] private TextMeshProUGUI m_discoveryPercentageText;
        
        private HologramBody m_highlightedBody;
        private Animator m_animator;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public override void SetHologramBody(HologramBody body)
        {
            base.SetHologramBody(body);
            
            m_highlightedBody = body;
            
            gameObject.SetActive(true);
            //transform.position = body.transform.position;

            m_bodyNameText.text = body.Signal.GetBodyName();
            m_bodyTypeText.text = body.Signal.GetBodyType();

            m_discoveryPercentageText.text = body.Signal.DiscoveryPercentage.ToString() + "%";
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
