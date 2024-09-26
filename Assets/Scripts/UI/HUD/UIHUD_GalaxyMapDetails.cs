using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects.Astro;
using UnityEngine;

namespace Corruption.UI.HUD
{
    public class UIHUD_GalaxyMapDetails : UIWindow
    {
        [SerializeField] private GalaxyMap m_galaxyMap;
        [SerializeField] private SystemMap m_systemMap;
        
        [Space, SerializeField] private UIPANEL_MarkerDetails m_markerDetails;
        [SerializeField] private UIPANEL_SystemBodyDetails m_systemBodyDetails;

        private void Start()
        {
            m_galaxyMap.OnExitGalaxyMap += OnExitGalaxyMap;
            
            m_systemMap.OnSystemBodySelected += OnSystemBodySelected;
            m_systemMap.OnSystemBodyDeselected += OnSystemBodyDeselected;
        }

        private void OnExitGalaxyMap()
        {
            m_markerDetails.gameObject.SetActive(false);
            m_systemBodyDetails.gameObject.SetActive(false);
        }

        private void OnSystemBodySelected(SystemBodyHologram systemBodyHologram)
        {
            m_systemBodyDetails.SetSystemBodyHologram(systemBodyHologram);
            m_systemBodyDetails.gameObject.SetActive(true);
        }

        private void OnSystemBodyDeselected(SystemBodyHologram systemBodyHologram)
        {
            m_systemBodyDetails.gameObject.SetActive(false);
        }
    }
}
