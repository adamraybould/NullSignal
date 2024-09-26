using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects;
using UnityEngine;

namespace Corruption.UI
{
    public abstract class UIHologram_Panel : MonoBehaviour
    {
        [SerializeField] protected SystemMapHologram m_hologram;
        [SerializeField] private bool m_shouldFacePlayer = true;
        
        protected Camera m_camera;

        protected virtual void Start()
        {
            m_camera = Camera.main;
        }

        protected virtual void Update()
        {
            if (m_shouldFacePlayer)
            {
                // Rotate Towards the Player
                Quaternion rotation = m_camera.transform.rotation;
                transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
            }
        }

        public virtual void SetHologramBody(HologramBody body)
        {
            
        }
    }
}
