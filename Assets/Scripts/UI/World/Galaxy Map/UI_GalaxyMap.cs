using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Objects;
using Corruption.Objects.Astro;
using Corruption.Objects.Displays;
using Corruption.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Corruption.UI.World
{
    public class UI_GalaxyMap : MonoBehaviour
    {
        [Header("Core")] 
        [SerializeField] private EchoRadar m_radar;
        [SerializeField] private GalaxyMap m_galaxyMap;
        [SerializeField] private SystemMap m_systemMap;
        [SerializeField] private GalaxyMapCamera m_mapCamera;
        [SerializeField] private EchoSpawner m_echoSpawner; 
        
        [Header("UI")]
        [SerializeField] private Image m_mapGrid;
        [SerializeField] private Image m_cameraCentre;
        
        [Space,SerializeField] private Transform m_echoWidgetContainer;
        [SerializeField] private Transform m_markerHighlight;

        [Space, SerializeField] private Transform m_starSystemWidgetContainer;
        
        private Material m_rippleMaterial;
        private float m_rippleMaterialTimer;
        
        private AudioPlayer m_audioPlayer;
        
        private void Awake()
        {
            m_audioPlayer = GetComponent<AudioPlayer>();
            
            m_rippleMaterial = m_mapGrid.material;
            m_mapGrid.pixelsPerUnitMultiplier = m_mapGrid.sprite.rect.width / 100;
        }

        private void Start()
        {
            m_radar.OnPulse += OnPulse;
            m_radar.OnPulseReset += OnPulseReset;
            
            m_galaxyMap.OnMarkerHighlighted += OnMarkerHighlighted;
            m_galaxyMap.OnMarkerUnhighlighted += OnMarkerUnhighlighted;
            m_systemMap.OnEnterSystemMap += OnEnterSystemMap;
            m_systemMap.OnExitSystemMap += OnExitSystemMap;
            m_echoSpawner.OnSpawnEcho += OnEchoSpawn;
            
            InitialiseGalaxyMap();
        }

        private void OnEnable()
        {
            m_rippleMaterialTimer = 0.0f;
        }

        private void Update()
        {
            if (m_rippleMaterialTimer > 0)
            {
                m_rippleMaterialTimer += Time.deltaTime;
                m_rippleMaterial.SetFloat("_Timer", m_rippleMaterialTimer);
            }
        }

        private void InitialiseGalaxyMap()
        {
            List<StarSystem> discoveredSystems = m_galaxyMap.GalaxyMapData.DiscoveredStarSystems;
            foreach (StarSystem starSystem in discoveredSystems)
            {
                StarSystemMarker systemMarker = ObjectPool.Instance.GetPooledObject<StarSystemMarker>("SystemMarker", m_starSystemWidgetContainer);
                UI_StarSystemMarker markerWidget = systemMarker.GetComponent<UI_StarSystemMarker>();
                
                systemMarker.AssignSystem(starSystem);
                systemMarker.SetPosition(new Vector3(0.0f, 20.0f, 0.0f));
                markerWidget.SetCamera(m_mapCamera.Camera);
                markerWidget.Display(); 
            }
        }

        private void OnPulse(float duration)
        {
            //StartCoroutine(RippleEffect(duration));
            
            m_rippleMaterial.SetFloat("_Speed", m_radar.PulseSpeed * 0.1f);
            m_rippleMaterial.SetFloat("_Active", 1);
            m_rippleMaterial.SetVector("_Origin", GetRippleEffectOrigin());
            m_rippleMaterialTimer = 0.001f;
            
            m_audioPlayer.Play(0);
        }

        private void OnPulseReset()
        {
            StopAllCoroutines();
            
            m_rippleMaterialTimer = 0.0f;
            m_rippleMaterial.SetFloat("_Active", 0);
        }

        private void OnEchoSpawn(Vector3 echoPosition, Echo echo)
        {
            EchoMarker echoMarker = ObjectPool.Instance.GetPooledObject<EchoMarker>("EchoMarker", m_echoWidgetContainer);
            UI_EchoMarker echoWidget = echoMarker.GetComponent<UI_EchoMarker>();
            
            echoMarker.AttachEcho(echo);
            echoMarker.SetPosition(new Vector3(echoPosition.x, Mathf.Abs(echoPosition.y), echoPosition.z));
            echoWidget.SetCamera(m_mapCamera.Camera);
        }

        private void OnMarkerHighlighted(StellarMarker marker)
        {
            m_markerHighlight.gameObject.SetActive(true);
            m_markerHighlight.transform.position = marker.transform.position;
        }

        private void OnMarkerUnhighlighted(StellarMarker marker)
        {
            m_markerHighlight.gameObject.SetActive(false);
        }

        private void OnEnterSystemMap()
        {
            m_echoWidgetContainer.gameObject.SetActive(false);
            m_starSystemWidgetContainer.gameObject.SetActive(false);
            //m_cameraCentre.gameObject.SetActive(false);
        }

        private void OnExitSystemMap()
        {
            m_echoWidgetContainer.gameObject.SetActive(true);
            m_starSystemWidgetContainer.gameObject.SetActive(true);
            //m_cameraCentre.gameObject.SetActive(true);
        }

        private IEnumerator RippleEffect(float duration)
        {
            m_rippleMaterial.SetFloat("_Speed", m_radar.PulseSpeed * 0.1f);
            m_rippleMaterial.SetFloat("_Active", 1);
            m_rippleMaterial.SetVector("_Origin", GetRippleEffectOrigin());
            m_rippleMaterialTimer = 0.001f;
            
            yield return new WaitForSeconds(duration);
            
            m_rippleMaterialTimer = 0.0f;
            m_rippleMaterial.SetFloat("_Active", 0);
        }

        private Vector2 GetRippleEffectOrigin()
        {
            float effectiveWidth = m_mapGrid.rectTransform.rect.width;
            float effectiveHeight = m_mapGrid.rectTransform.rect.height;
            
            Vector3 localPosition = m_mapCamera.transform.position - m_mapGrid.transform.position;
            float u = ((localPosition.x + (effectiveWidth / 2)) / effectiveWidth) * 100.0f;
            float v = ((localPosition.z + (effectiveHeight / 2)) / effectiveHeight) * 100.0f;
            
            return new Vector2(u, v);
        }
    }
}
