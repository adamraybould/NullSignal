using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Corruption.Objects.Astro
{
    public class SystemBodyHologram : MonoBehaviour
    {
        [Serializable]
        private struct SystemBodyHologramMaterials
        {
            public Material Undiscovered;
        }
        
        public SystemBody Body { get; private set; }
        public SystemBody Primary { get; private set; }
        
        [SerializeField] private CinemachineVirtualCamera m_vCamera;
        [SerializeField] private Transform m_vCameraPivot;
        
        [Header("Body Properties")] 
        [SerializeField] private OrbitParameters m_orbitParameters;
        [SerializeField] private float m_bodySize;
        [SerializeField] private float m_distanceFromPrimary;

        [Space, SerializeField] private SystemBodyHologramMaterials m_hologramMaterials;
        
        private float m_currentOrbit;
        private bool m_CanOrbit = true;
        private Vector3 m_initialPosition;
        private float m_initialTrailTime;
        
        private LineRenderer m_lineRenderer;
        private TrailRenderer m_trailRenderer;
        private Renderer m_renderer;
        private Material m_material;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_trailRenderer = GetComponent<TrailRenderer>();
            m_renderer = GetComponent<Renderer>();
            m_material = m_renderer.material;

            m_initialTrailTime = m_trailRenderer.time;
        }
        
        private void OnEnable()
        {
            ResetOrbit();
        }

        private void OnDisable()
        {
            m_trailRenderer.Clear();
        }

        private void Update()
        {
            Orbit();
        }

        public void AssignBody(SystemBody body, SystemBodyHologram parent)
        {
            Body = body;
            Primary = parent != null ? parent.Body : null;

            gameObject.name = body.Body.Name.Replace(" ", "_");
            m_orbitParameters = body.OrbitParameters;
            m_bodySize = body.Size;
            m_distanceFromPrimary = body.DistanceFromPrimary;

            m_trailRenderer.time = m_initialTrailTime / m_orbitParameters.OrbitSpeed;

            if (body.Body.BodyMaterial)
            {
                m_renderer.material = body.Body.BodyMaterial;
            }
            else
            {
                m_renderer.material = m_hologramMaterials.Undiscovered;
            }

            SetParent(parent);
            SetScale(m_bodySize);
        }

        public void Select(GalaxyMapCamera mapCamera)
        {
            m_vCameraPivot.transform.localRotation = mapCamera.GetCameraRotation();
            mapCamera.SetCameraFocus(m_vCameraPivot);
            m_vCamera.enabled = true;
        }

        public void Deselect(GalaxyMapCamera mapCamera)
        {
            mapCamera.ResetCameraFocus();
            m_vCamera.enabled = false;
        }

        private void Orbit()
        {
            if (!m_CanOrbit || m_orbitParameters.OrbitSpeed == 0.0f)
                return;

            // -- Reset Orbit
            if (m_currentOrbit > 360.0f)
            {
                m_currentOrbit = 0.0f;
            }

            m_currentOrbit += m_orbitParameters.OrbitSpeed * Time.deltaTime;
            CalculateOrbit();
        }

        private void SetParent(SystemBodyHologram parent)
        {
            if (parent != null)
            {
                transform.parent = parent.transform;
                transform.localPosition = new Vector3(m_distanceFromPrimary * m_bodySize, 0.0f, 0.0f);
                m_initialPosition = transform.localPosition;
            }
            
            SetOrbitCircle();
        }

        private void SetScale(float scale)
        {
            float bodySize = Primary != null ? scale / 2 : scale;
            transform.localScale = new Vector3(bodySize, bodySize, bodySize);
        }

        private void ResetOrbit()
        {
            m_currentOrbit = Random.Range(0, 360.0f);
            CalculateOrbit();
        }

        private void CalculateOrbit()
        {
            float orbitInRadians = m_currentOrbit * Mathf.Deg2Rad;
            
            float x = Mathf.Cos(orbitInRadians) * m_initialPosition.x - Mathf.Sin(orbitInRadians) * m_initialPosition.z;
            float z = Mathf.Sin(orbitInRadians) * m_initialPosition.x - Mathf.Cos(orbitInRadians) * m_initialPosition.z;
            transform.localPosition = new Vector3(x, m_initialPosition.y, z);
        }

        private void SetOrbitCircle()
        {
            if (Primary == null)
                return;

            float distanceToPrimary = Vector3.Distance(transform.position, transform.parent.position);

            int segments = 100;
            m_lineRenderer.positionCount = segments + 1;

            float angle = 0.0f;
            for (int i = 0; i < (segments + 1); i++)
            {
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * distanceToPrimary;
                float z = Mathf.Cos(Mathf.Deg2Rad * angle) * distanceToPrimary;
                
                m_lineRenderer.SetPosition(i, new Vector3(transform.parent.position.x + x, transform.parent.position.y, transform.parent.position.z + z));
                angle += (360.0f / segments);
            }
        }
    }
}
