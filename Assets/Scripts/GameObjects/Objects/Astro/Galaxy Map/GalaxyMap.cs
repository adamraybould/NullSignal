using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects.Astro
{
    public class GalaxyMap : MonoBehaviour
    {
        public event Action OnEnterGalaxyMap;
        public event Action OnExitGalaxyMap;

        public event Action<StellarMarker> OnMarkerSelected;
        public event Action<StellarMarker> OnMarkerHighlighted;
        public event Action<StellarMarker> OnMarkerUnhighlighted; 

        public GalaxyMapData GalaxyMapData { get; private set; }
        public bool IsViewingSystemMap { get; private set; }
        
        [SerializeField] private EchoRadar m_radar;
        [SerializeField] private GalaxyMapCamera m_mapCamera;
        [SerializeField] private SystemMap m_systemMap;
        
        private StellarMarker m_highlightedMarker = null;
        private StellarMarker m_selectedMarker = null;

        private void Awake()
        {
            GalaxyMapData = GetComponent<GalaxyMapData>();
        }

        private void Start()
        {
            PlayerController.Input.GalaxyMap.Pulse.performed += ctx => Pulse();
            PlayerController.Input.GalaxyMap.Select.performed += ctx => SelectMarker();
            m_systemMap.OnExitSystemMap += OnSystemMapClose;
        }

        private void Update()
        {
            if (IsViewingSystemMap)
                return;
            
            RaycastHit hit;
            Ray ray = m_mapCamera.Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 500.0f, 1 << 10))
            {
                if (!m_highlightedMarker || hit.collider.gameObject != m_highlightedMarker.gameObject)
                {
                    if (m_highlightedMarker)
                    {
                        DehighlightMarker();
                    }

                    m_highlightedMarker = hit.collider.gameObject.GetComponent<StellarMarker>();
                    m_highlightedMarker.Highlight(this);
                    OnMarkerHighlighted?.Invoke(m_highlightedMarker);
                }
            }
            else
            {
                DehighlightMarker();
            }
        }
        
        public void OpenSystemMap(StarSystem starSystem)
        {
            IsViewingSystemMap = true;
            DehighlightMarker();
            
            m_systemMap.DisplaySystemMap(starSystem, m_mapCamera);
        }
        
        private void Pulse()
        {
            if (IsViewingSystemMap)
                return;
            
            m_radar.Pulse();
        }

        private void SelectMarker()
        {
            if (m_highlightedMarker && !IsViewingSystemMap)
            {
                m_selectedMarker = m_highlightedMarker;
                OnMarkerSelected?.Invoke(m_selectedMarker);

                m_selectedMarker.Select(this);
                
                //StarSystem starSystem = m_selectedEcho.Source as StarSystem;
                //DisplayStarSystem(starSystem);
                //m_radar.TriangulateEcho(m_selectedEcho);
            }
        }
        
        private void DehighlightMarker()
        {
            if (!m_highlightedMarker)
                return;
            
            m_highlightedMarker.Dehighlight(this);
            OnMarkerUnhighlighted?.Invoke(m_highlightedMarker);
            m_highlightedMarker = null;
        }

        private void OnSystemMapClose()
        {
            DehighlightMarker();
            m_selectedMarker = null;
            IsViewingSystemMap = false;
        }

        public void Activate()
        {
            gameObject.SetActive(true); 
            m_mapCamera.SetCameraActive(true);
            DehighlightMarker();
            
            OnEnterGalaxyMap?.Invoke();
        }

        public void Deactivate()
        {
            if (IsViewingSystemMap)
                return;
            
            m_highlightedMarker = null;
            m_selectedMarker = null;
            
            m_mapCamera.SetCameraActive(false);
            OnExitGalaxyMap?.Invoke();
        }
    }
}
