using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using Corruption.Core.Input;
using Corruption.UI;
using UnityEngine;

namespace Corruption.Entities.Player.Components
{
    public class PlayerInteractor : MonoBehaviour, IPlayerComponent
    {
        public event Action<IInteractable> OnInteract;
        public event Action<Focusable> OnFocus;
        
        public bool IsFocused { get; private set; }
        
        [SerializeField] private UI_InteractPrompt m_interactPromptUI;
        
        [SerializeField] private float m_interactRange = 2.0f;
        [SerializeField] private PlayerCamera m_playerCamera;

        private GameObject m_focusedObject = null;
        private IInteractable m_interactableObject = null;
        
        private void Start()
        {
            PlayerController.Interact += Interact;
            PlayerController.ExitFocus += ExitFocus;
        }

        private void FixedUpdate()
        {
            if (!IsFocused)
            {
                RaycastHit hit;
                Debug.DrawRay(m_playerCamera.GetCameraTransform().position, m_playerCamera.GetCameraTransform().TransformDirection(Vector3.forward) * m_interactRange, Color.yellow);
                if (Physics.Raycast(m_playerCamera.GetCameraTransform().position, m_playerCamera.GetCameraTransform().TransformDirection(Vector3.forward), out hit, m_interactRange, 1 << 6))
                {
                    // Only check if focused on a different game object
                    if (m_focusedObject != hit.collider.gameObject)
                    {
                        // Run if no objects can be found that are Interactable
                        CloseUIPrompt();

                        m_focusedObject = hit.collider.gameObject;
                        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                        if (interactable != null)
                        {
                            m_interactableObject = interactable;
                            m_interactPromptUI.Display(m_focusedObject, m_interactableObject.InteractionPrompt);
                        }
                    }

                    return;
                }
                
                m_focusedObject = null;
                CloseUIPrompt();
            }
            
            CloseUIPrompt();
        }

        private void Interact()
        {
            if (IsFocused)
                return;
                
            m_interactableObject?.Interact();
            OnInteract?.Invoke(m_interactableObject);
        }

        private void ExitFocus()
        {
            if (!IsFocused)
                return;
            
            Focusable focusedItem = m_focusedObject.GetComponent<Focusable>();
            if (focusedItem.UnFocus())
            {
                m_focusedObject = null;
            }
        }

        private void CloseUIPrompt()
        {
            if (m_interactPromptUI.IsDisplayed)
            {
                m_interactableObject = null;
                m_interactPromptUI.Hide();
            }
        }

        public void OnInteractiveFocusEnter()
        {
            IsFocused = true;
        }

        public void OnInteractiveFocusExit()
        {
            IsFocused = false;
        }
    }
}
