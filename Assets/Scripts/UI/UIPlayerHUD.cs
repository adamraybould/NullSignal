using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI
{
    public class UIPlayerHUD : UICanvas
    {
        public bool MouseLocked { get; private set; }
        
        [Header("HUD")]
        [SerializeField] private Image m_reticle;

        private void Start()
        {
            GameEvents.RequestLockMouse.Event += LockMouse;
        }

        public void LockMouse(bool value)
        {
            MouseLocked = value;
            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.Confined;
            m_reticle.gameObject.SetActive(value);
        }
    }
}
