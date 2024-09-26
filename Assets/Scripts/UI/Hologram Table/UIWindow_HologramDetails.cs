using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using Corruption.Objects;
using UnityEngine;

namespace Corruption.UI
{
    public class UIWindow_HologramDetails : UIWindow
    {
        public SystemMapHologram Hologram => m_hologram;
        
        [SerializeField] protected SystemMapHologram m_hologram;
        
        [SerializeField] private UIHologram_StarDetails m_starDetails;
        [SerializeField] private UIHologram_BodyDetails m_bodyDetails;

        private void Start()
        {
            m_starDetails.gameObject.SetActive(false);
            m_bodyDetails.gameObject.SetActive(false);
            
            m_hologram.OnHologramBodySelection += OnHologramBodySelected;
            m_hologram.OnHologramBodySelectionBack += OnHologramBodySelectionBack;
        }
        
        private void OnHologramBodySelected(HologramBody body)
        {
            switch (body.StellarBody.Type)
            {
                case StellarBodyType.STAR:
                {
                    m_starDetails.gameObject.SetActive(true);
                    m_bodyDetails.gameObject.SetActive(false);
                    
                    m_starDetails.SetHologramBody(body);
                    break;
                }

                case StellarBodyType.PLANET:
                {
                    m_starDetails.gameObject.SetActive(false);
                    m_bodyDetails.gameObject.SetActive(true);
                    
                    m_bodyDetails.SetHologramBody(body);
                    break;
                }
            }
        }

        private void OnHologramBodySelectionBack(HologramBody body)
        {
            if (m_starDetails.gameObject.activeInHierarchy)
            {
                m_starDetails.Close();
                m_bodyDetails.gameObject.SetActive(false);
            }
            else if (m_bodyDetails.gameObject.activeInHierarchy)
            {
                m_starDetails.gameObject.SetActive(false);
                m_bodyDetails.Close();
            }
        }
    }
}
