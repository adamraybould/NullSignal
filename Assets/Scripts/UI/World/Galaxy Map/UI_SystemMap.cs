using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects.Astro;
using Corruption.Utility;
using UnityEngine;

namespace Corruption.UI.World
{
    public class UI_SystemMap : MonoBehaviour
    {
        [Header("Core")] 
        [SerializeField] private SystemMap m_systemMap;

        [Header("UI")] 
        [SerializeField] private UI_Highlighter m_systemBodyHighlight;
        
        private AudioPlayer m_audioPlayer;

        private void Awake()
        {
            m_audioPlayer = GetComponent<AudioPlayer>();
        }

        private void Start()
        {
            m_systemMap.OnSystemBodyHighlighted += OnSystemBodyHighlighted;
            m_systemMap.OnSystemBodyUnHighlighted += OnSystemBodyUnHighlighted;
        }

        private void OnSystemBodyHighlighted(SystemBodyHologram bodyHologram)
        {
            if (m_systemMap.IsViewingSystemBody)
                return;
            
            m_systemBodyHighlight.gameObject.SetActive(true);
            m_systemBodyHighlight.transform.position = bodyHologram.transform.position;
            m_systemBodyHighlight.AssignFollowParent(bodyHologram.transform);
        }

        private void OnSystemBodyUnHighlighted(SystemBodyHologram bodyHologram)
        {
            m_systemBodyHighlight.gameObject.SetActive(false);
        }
    }
}
