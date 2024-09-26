using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects.Astro;
using TMPro;
using UnityEngine;

namespace Corruption.UI.HUD
{
    public class UIPANEL_SystemBodyDetails : MonoBehaviour
    {
        [SerializeField] private SystemMap m_systemMap;

        [Header("UI")] 
        [SerializeField] private Transform m_bodyDetailsContainer;
        [SerializeField] private TextMeshProUGUI m_bodyNameText;
        [SerializeField] private TextMeshProUGUI m_bodyTypeText;

        public void SetSystemBodyHologram(SystemBodyHologram systemBodyHologram)
        {
            m_bodyNameText.text = systemBodyHologram.Body.Body.Name;
            m_bodyTypeText.text = systemBodyHologram.Body.Body.GetBodyType();
        }
        
    }
}
