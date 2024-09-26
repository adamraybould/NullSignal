using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class SystemMap : MonoBehaviour
    {
        public event Action OnEnterSystemMap;
        public event Action OnExitSystemMap;
        
        public event Action<SystemBodyHologram> OnSystemBodySelected;
        public event Action<SystemBodyHologram> OnSystemBodyDeselected;
        public event Action<SystemBodyHologram> OnSystemBodyHighlighted;
        public event Action<SystemBodyHologram> OnSystemBodyUnHighlighted;
     
        public StarSystem ActiveStarSystem { get; private set; }
        public bool IsViewingSystemBody { get; private set; }
        
        [Space, SerializeField] private Transform m_hologramBodyContainer;
        [SerializeField] private List<SystemBodyHologram> m_bodyHolograms;

        private GalaxyMapCamera m_mapCamera;
        
        private SystemBodyHologram m_highlightedSystemBody;
        private SystemBodyHologram m_selectedSystemBody;
        
        private void Start()
        {
            if (m_bodyHolograms.Count <= 0)
            {
                Debug.LogError("No Hologram Bodies Defined in System Map");
                return;
            }

            PlayerController.Input.SystemMap.Select.performed += ctx => SelectSystemBody();
            PlayerController.Input.SystemMap.Back.performed += ctx => Back();
        }

        private void Update()
        {
            RaycastHit hit;
            Ray ray = m_mapCamera.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 500.0f, 1 << 9))
            {
                if (!m_highlightedSystemBody || hit.collider.gameObject != m_highlightedSystemBody.gameObject)
                {
                    if (m_highlightedSystemBody)
                    {
                        UnHighlightSystemBody();
                    }

                    m_highlightedSystemBody = hit.collider.gameObject.GetComponent<SystemBodyHologram>();
                    OnSystemBodyHighlighted?.Invoke(m_highlightedSystemBody);
                }
            }
            else
            {
                UnHighlightSystemBody();
            }
        }

        public void DisplaySystemMap(StarSystem starSystem, GalaxyMapCamera galaxyMapCamera)
        {
            ActiveStarSystem = starSystem;
            m_mapCamera = galaxyMapCamera;
            gameObject.SetActive(true);
            
            ResetSystemMap();
            DisplaySystemBodies(ActiveStarSystem);
            
            OnEnterSystemMap?.Invoke();
        }
        
        public void CloseSystemMap()
        {
            gameObject.SetActive(false);
            ResetSystemMap();
            
            OnExitSystemMap?.Invoke();
        }

        private void Back()
        {
            if (IsViewingSystemBody)
            {
                DeselectSystemBody();
            }
            else
            {
                CloseSystemMap();
            }
        }

        private void SelectSystemBody()
        {
            if (m_highlightedSystemBody && !m_selectedSystemBody && !IsViewingSystemBody)
            {
                IsViewingSystemBody = true;
                m_selectedSystemBody = m_highlightedSystemBody;
                m_selectedSystemBody.Select(m_mapCamera);
                
                OnSystemBodySelected?.Invoke(m_selectedSystemBody);
            }
        }

        private void DeselectSystemBody()
        {
            if (m_selectedSystemBody && IsViewingSystemBody)
            {
                IsViewingSystemBody = false;
                m_selectedSystemBody.Deselect(m_mapCamera);

                OnSystemBodyDeselected?.Invoke(m_selectedSystemBody);
                m_selectedSystemBody = null;
            }
        }
        
        private void DisplaySystemBodies(StarSystem starSystem)
        {
            // -- Activate Required Hologram Bodies
            if (m_bodyHolograms.Count > 0)
            {
                if (m_bodyHolograms.Count < starSystem.GetBodyCount())
                {
                    Debug.LogError("Not Enough Hologram Bodies for System '" + starSystem.SystemName + "'");
                    return;
                }

                foreach (SystemBody star in starSystem.Stars)
                {
                    AssignHologramBody(star, null);
                }
            }
        }
        
        private void AssignHologramBody(SystemBody body, SystemBodyHologram parent)
        {
            SystemBodyHologram bodyHologram = GetAvailableHologram();
            bodyHologram.gameObject.SetActive(true);
            bodyHologram.AssignBody(body, parent);

            foreach (SystemBody orbitingBody in body.OrbitingBodies)
            {
                AssignHologramBody(orbitingBody, bodyHologram);
            }
        }
        
        private SystemBodyHologram GetAvailableHologram()
        {
            foreach (SystemBodyHologram hologram in m_bodyHolograms)
            {
                if (!hologram.gameObject.activeInHierarchy)
                {
                    return hologram;
                }
            }

            Debug.LogError("ERROR: No Available Hologram Bodies");
            return null;
        }
        
        private void UnHighlightSystemBody()
        {
            if (!m_highlightedSystemBody)
                return;

            OnSystemBodyUnHighlighted?.Invoke(m_highlightedSystemBody);
            m_highlightedSystemBody = null;
        }
        
        private void ResetSystemMap()
        {
            foreach (SystemBodyHologram hologram in m_bodyHolograms)
            {
                hologram.gameObject.SetActive(false);
            }
        }
    }
}
