using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using UnityEngine;

namespace Corruption.Objects
{
    public class RadarDetectionPoint : MonoBehaviour
    {
        public Vector3 SignalPosition { get; private set; } // The Collider of a Signal Object

        private Collider m_signalCollider;
        private SphereCollider m_collider;

        private void Awake()
        {
            m_collider = GetComponent<SphereCollider>();
        }

        public void DetectSignals()
        {
            float maxDistance = float.MaxValue;
            Collider closestSignalCollider = null;
            
            Collider[] signalColliders = Physics.OverlapSphere(transform.position, m_collider.radius, 1 << 7);
            if (signalColliders.Length <= 0)
            {
                SignalPosition = Vector3.zero;
                m_signalCollider = null;
            }
            
            foreach (Collider signalCollider in signalColliders)
            {
                float distance = Vector2.Distance(transform.position, signalCollider.transform.position);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    closestSignalCollider = signalCollider;
                }
            }

            m_signalCollider = closestSignalCollider;
            SignalPosition = closestSignalCollider.transform.position;
        }

        public Echo GetClosestSignal(float range)
        {
            if (m_signalCollider == null)
                return null;

            if (Vector3.Distance(transform.position, m_signalCollider.transform.position) > range)
                return null;
            
            return m_signalCollider.GetComponent<SignalVisual>().Echo;
        }
    }
}
