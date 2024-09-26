using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Corruption.Core.Framework
{
    public class Focusable : MonoBehaviour, IInteractable
    {
        public event Action OnFocusEnter;
        public event Action OnFocusExit;
     
        public bool Focused { get; private set; }
        
        public string InteractionPrompt => m_prompt;
        [SerializeField] private string m_prompt;

        [SerializeField] private CinemachineVirtualCamera m_virtualCamera;
        
        public virtual bool Interact()
        {
            Focus();
            return true;
        }
        
        public virtual void Focus()
        {
            Focused = true;
            m_virtualCamera.enabled = true;
            GameEvents.PlayerInteractionFocusEnter.Trigger();
            GameEvents.RequestLockMouse.Trigger(false);
            
            OnFocusEnter?.Invoke();
        }

        public virtual bool UnFocus()
        {
            Focused = false;
            m_virtualCamera.enabled = false;
            GameEvents.PlayerInteractionFocusExit.Trigger();
            GameEvents.RequestLockMouse.Trigger(true);
            
            OnFocusExit?.Invoke();
            return true;
        }
    }
}
