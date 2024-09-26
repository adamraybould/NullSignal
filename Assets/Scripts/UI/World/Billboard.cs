using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.UI.World
{
    public class Billboard : MonoBehaviour
    {
        [Serializable]
        public struct BillboardDistanceScaling
        {
            public bool ScaleWithDistance;
            public MinMaxRange DistanceRange;
            public MinMaxRange ScaleRange;
        }
        
        [Serializable]
        public struct BillboardFreezeRotation
        {
            public int X { get { return m_x ? 0 : 1; }}
            public int Y { get { return m_y ? 0 : 1; }}
            public int Z { get { return m_z ? 0 : 1; }}
            
            [SerializeField] private bool m_x;
            [SerializeField] private bool m_y;
            [SerializeField] private bool m_z;
        }

        public Camera Camera => m_camera;
        
        [SerializeField] private Camera m_camera;
        [SerializeField] private BillboardDistanceScaling m_distanceScaling;
        [SerializeField] private BillboardFreezeRotation m_freezeRotation;

        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (m_camera == null)
            {
                SetCamera(Camera.main);
            }
        }

        private void Update()
        {
            LookAtCamera();
            ScaleWithDistance();
        }

        public void SetCamera(Camera camera)
        {
            m_camera = camera;
        }

        private void LookAtCamera()
        {
            Quaternion rotation = m_camera.transform.rotation;
            rotation = new Quaternion(rotation.x * m_freezeRotation.X, rotation.y * m_freezeRotation.Y, rotation.z * m_freezeRotation.Z, rotation.w);
            
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }

        private void ScaleWithDistance()
        {
            if (!m_distanceScaling.ScaleWithDistance)
                return;

            float distanceToCamera = Vector3.Distance(transform.position, m_camera.transform.position);
            float clampedDistance = Mathf.Clamp(distanceToCamera, m_distanceScaling.DistanceRange.Min, m_distanceScaling.DistanceRange.Max);
            float t = (clampedDistance - m_distanceScaling.DistanceRange.Min) / (m_distanceScaling.DistanceRange.Max - m_distanceScaling.DistanceRange.Min);
            
            Vector3 minScale = Vector3.one * m_distanceScaling.ScaleRange.Min;
            Vector3 maxScale = Vector3.one * m_distanceScaling.ScaleRange.Max;
            Vector3 newScale = Vector3.Lerp(minScale, maxScale, t);

            m_rectTransform.localScale = newScale;
        }
    }
}
