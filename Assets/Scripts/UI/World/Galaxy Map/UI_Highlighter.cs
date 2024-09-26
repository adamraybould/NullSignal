using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.UI.World
{
    public class UI_Highlighter : MonoBehaviour
    {
        [SerializeField] private MinMaxRange m_distance;
        [SerializeField] private MinMaxRange m_distanceScale;

        private RectTransform m_rectTransform;
        private Billboard m_billboard;
        private Camera m_mapCamera;

        private Transform m_parent; // The Transform to Follow
        
        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_billboard = GetComponent<Billboard>();
            m_mapCamera = m_billboard.Camera;
        }

        private void Update()
        {
            if (m_parent)
            {
                transform.position = m_parent.position;
            }
            
            float distance = Vector3.Distance(transform.position, m_mapCamera.transform.position);
            float clampedDistance = Mathf.Clamp(distance, m_distance.Min, m_distance.Max);
            
            float t = (clampedDistance - m_distance.Min) / (m_distance.Max - m_distance.Min);

            Vector2 minScale = Vector2.one * m_distanceScale.Min;
            Vector2 maxScale = Vector2.one * m_distanceScale.Max;
            Vector2 scale = Vector2.Lerp(minScale, maxScale, t);

            m_rectTransform.sizeDelta = scale;
        }

        public void AssignFollowParent(Transform parent)
        {
            m_parent = parent;

            float scale = Mathf.Clamp(parent.localScale.x, 0.5f, 1.5f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
