using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.Core.Input
{
    public class UIMenuController : MonoBehaviour
    {
        public static event Action OnEscape;
        
        public static event Action OnClick;
        public static event Action OnDoubleClick;

        public static MenuControls Input { get; private set; }
        
        [SerializeField] private float m_clickCheckDuration; // The amount of time a double click can be preformed within
        [SerializeField] private int m_clickCount; // Keeps track of how many times the Player has clicked on this Body
        private float m_clickTimer;

        private void Awake()
        {
            SetUpInput();
            
            m_clickTimer = m_clickCheckDuration;
        }
        
        private void Update()
        {
            if (m_clickCount != 0)
            {
                m_clickTimer -= Time.deltaTime;
                if (m_clickTimer <= 0)
                {
                    ResetClickCount();
                }
            }
        }

        public void OnMouseClick()
        {
            m_clickCount++;
            if (m_clickCount <= 1)
            {
                OnClick?.Invoke();
            } 
            else
            {
                OnDoubleClick?.Invoke();
                ResetClickCount();
            }
        }

        private void ResetClickCount()
        {
            m_clickCount = 0;
            m_clickTimer = m_clickCheckDuration;
        }

        private static void SetUpInput()
        {
            Input = new MenuControls();
            Input.Enable();

            Input.Menu.Escape.performed += ctx => OnEscape?.Invoke();
        }
        
        private void OnEnable() { Input.Enable(); }
        private void OnDisable() { Input.Disable(); }
    }
}
