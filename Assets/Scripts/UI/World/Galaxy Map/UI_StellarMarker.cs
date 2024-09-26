using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects.Astro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI.World
{
    [RequireComponent(typeof(StellarMarker))]
    public class UI_StellarMarker : MonoBehaviour
    {
        [Header("Stellar Marker")] 
        [SerializeField] private List<Billboard> m_billboards;
        [SerializeField] private Transform m_visualTransform;
        
        [SerializeField] private Image m_markerPole;
        [SerializeField] private Transform m_selectableSection;

        private Camera m_mainCamera;
        protected StellarMarker m_stellarMarker;

        protected virtual void Awake()
        {
            m_stellarMarker = GetComponent<StellarMarker>();
        }

        protected virtual void Start()
        {
            SetPoleLength();

            m_stellarMarker.OnHighlighted += OnHighlighted;
            m_stellarMarker.OnDehighlighted += OnDehighlighted;
        }

        public virtual void Display()
        {
            m_visualTransform.gameObject.SetActive(true);
        }

        public void SetCamera(Camera mainCamera)
        {
            m_mainCamera = mainCamera;
            SetBillboardCamera();
        }

        private void SetPoleLength()
        {
            float distanceToBottom = Vector3.Distance(transform.position, new Vector3(transform.position.x, 0.0f, transform.position.z));
            float offsetLength = distanceToBottom;

            m_markerPole.rectTransform.pivot = new Vector2(m_markerPole.rectTransform.pivot.x, IsMarkerBelowGrid() ? 0 : 1);
            m_markerPole.rectTransform.sizeDelta = new Vector2(m_markerPole.rectTransform.sizeDelta.x, offsetLength);
        }

        private void SetBillboardCamera()
        {
            foreach (Billboard billboard in m_billboards)
            {
                billboard.SetCamera(m_mainCamera);
            }
        }

        private void OnHighlighted()
        {
            m_selectableSection.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }

        private void OnDehighlighted()
        {
            m_selectableSection.transform.localScale = Vector3.one;
        }
        
        protected bool IsMarkerBelowGrid() { return transform.position.y < 0; }
    }
}
