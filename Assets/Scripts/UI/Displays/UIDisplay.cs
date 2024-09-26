using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Objects.Interactive;
using UnityEngine;

namespace Corruption.UI.Display
{
    public abstract class UIDisplay : UICanvas
    {
        public Monitor Monitor => m_monitor;
        
        public bool IsActive { get; private set; }
        public bool IsFocused { get; private set; }
        
        [SerializeField] private Monitor m_monitor;

        protected virtual void Start()
        {
            m_monitor.OnTurnedOn += OnTurnedOn;
            m_monitor.OnTurnedOff += OnTurnedOff;
            m_monitor.OnFocusEnter += OnFocusEnter;
            m_monitor.OnFocusExit += OnFocusExit;
        }

        protected virtual void OnTurnedOn() {}
        protected virtual void OnTurnedOff() {}
        protected virtual void OnFocusEnter() {}
        protected virtual void OnFocusExit() {}
    }
}
