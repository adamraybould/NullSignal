using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI
{
    public class UIHologram_BodyHighlight : UIHologram_Panel
    {
        [SerializeField] private TextMeshProUGUI m_bodyNameText;
        
        private HologramBody m_highlightedBody;
        private Vector3 m_initialScale;

        [SerializeField] private float m_scaleFactor;
        
        private void Awake()
        {
            m_initialScale = transform.localScale;
        }

        protected override void Start()
        {
            base.Start();
            
            m_hologram.OnHologramBodyHoverEnter += OnHologramBodyHoverEnter;
            m_hologram.OnHologramBodyHoverExit += OnHologramBodyHoverExit;
            gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();
            
            // Move with Hologram Body
            transform.position = m_highlightedBody.transform.position;
            
            // Scale with Distance from Camera
            ScaleWithDistance();
        }

        private void OnHologramBodyHoverEnter(HologramBody body)
        {
            gameObject.SetActive(true);
            m_highlightedBody = body;
            transform.position = body.transform.position;
            transform.localScale = Vector3.Scale(m_initialScale, body.transform.localScale);

            if (body.StellarBody)
            {
                if (body.Signal.Discovered)
                    m_bodyNameText.text = body.StellarBody.Name;
                else
                    m_bodyNameText.text = "???";
            }
        }

        private void OnHologramBodyHoverExit(HologramBody body)
        {
            gameObject.SetActive(false);
            m_highlightedBody = null;
            transform.position = Vector3.zero;
        }

        private void ScaleWithDistance()
        {
            if (m_highlightedBody)
            {
                float distance = Vector3.Distance(m_camera.transform.position, transform.position);
                float minScale = Vector3.Scale(m_initialScale, m_highlightedBody.transform.localScale).x - 0.2f;
                float maxScale = 1.0f;
                
                float newScale = Mathf.Lerp(minScale, maxScale, distance * m_scaleFactor);
                transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
    }
}
