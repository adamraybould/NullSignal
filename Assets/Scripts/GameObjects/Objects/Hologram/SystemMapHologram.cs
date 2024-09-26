using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core.Framework;
using Corruption.Core.Input;
using UnityEngine;

namespace Corruption.Objects
{
    public class SystemMapHologram : Focusable
    {
        public event Action<HologramBody> OnHologramBodyHoverEnter;
        public event Action<HologramBody> OnHologramBodyHoverExit;
        public event Action<HologramBody> OnHologramBodySelection;
        public event Action<HologramBody> OnHologramBodySelectionBack;
        
        [SerializeField] private RadarSignalProcesser m_signalProcessor;
        
        [Space, SerializeField] private GameObject m_systemHologram;
        [SerializeField] private List<HologramBody> m_hologramBodies;
        
        private Signal m_loadedSignal;
        
        private HologramBody m_highlightedBody;
        private HologramBody m_selectedBody;
        private bool m_bodySelected = false;
        
        private void Start()
        {
            foreach (HologramBody body in m_hologramBodies)
            {
                body.gameObject.SetActive(false);
            }
            
            m_signalProcessor.OnSignalDisplay += OnDisplaySignal;
            PlayerController.Input.HologramTable.Select.performed += ctx => OnDoubleClick();
            PlayerController.Input.HologramTable.Back.performed += ctx => OnBack();
        }

        public override bool UnFocus()
        {
            base.UnFocus();
            
            OnBack();
            DehighlightBody();
            return true;
        }

        private void FixedUpdate()
        {
            // If is in Use & a Body hasn't been selected
            if (Focused && !m_bodySelected)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 500.0f, 1 << 9))
                {
                    if (!m_highlightedBody || hit.collider.gameObject != m_highlightedBody.gameObject)
                    {
                        if (m_highlightedBody)
                        {
                            DehighlightBody();
                        }
                        
                        m_highlightedBody = hit.collider.gameObject.GetComponent<HologramBody>();
                        OnHologramBodyHoverEnter?.Invoke(m_highlightedBody);
                    }
                }
                else
                {
                    DehighlightBody();
                }
            }
            else
            {
                DehighlightBody();
            }
        }

        private void OnDisplaySignal(Signal signal)
        {
            ResetHologram();
            
            m_loadedSignal = signal;
            SetupHologram(signal);
            
            m_systemHologram.SetActive(true);
        }
                
        private void OnDoubleClick()
        {
            if (m_highlightedBody && !m_bodySelected)
            {
                m_bodySelected = true;
                m_selectedBody = m_highlightedBody;
                
                m_selectedBody.SelectBody(true);
                OnHologramBodySelection?.Invoke(m_selectedBody);
            }
        }

        private void OnBack()
        {
            if (m_bodySelected)
            {
                m_selectedBody.SelectBody(false);
                OnHologramBodySelectionBack?.Invoke(m_selectedBody);
                
                m_bodySelected = false;
                m_selectedBody = null;
            }
        }

        private void SetupHologram(Signal signal)
        {
            if (m_hologramBodies.Count <= 0)
                return;

            m_hologramBodies[0].gameObject.SetActive(true);
            m_hologramBodies[0].AttachSignal(signal);
            
            /*
            for (int i = 1; i < signal.StellarBody.OrbitingBodies.Count; i++)
            {
                m_hologramBodies[i].AttachSignal(signal.StellarBody.OrbitingBodies[i]);
                m_hologramBodies[i].gameObject.SetActive(true);
            }
            */
        }

        private void ResetHologram()
        {
            if (m_hologramBodies.Count <= 0)
                return;

            foreach (HologramBody body in m_hologramBodies)
            {
                body.gameObject.SetActive(false);
            }
        }

        private void DehighlightBody()
        {
            if (m_highlightedBody)
            {
                OnHologramBodyHoverExit?.Invoke(m_highlightedBody);
                m_highlightedBody = null;
            }
        }
    }
}
