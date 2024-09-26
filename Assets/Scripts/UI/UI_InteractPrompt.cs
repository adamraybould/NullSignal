using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Corruption.UI
{
    public class UI_InteractPrompt : MonoBehaviour
    {
        public bool IsDisplayed { get; private set; }
        
        [SerializeField] private TextMeshProUGUI m_text;
        private Image m_image;
        private Camera m_camera;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        private void Start()
        {
            m_camera = Camera.main;
            Hide();
        }

        private void LateUpdate()
        {
            Quaternion rotation = m_camera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
        }

        public void Display(GameObject interactableObject, string text)
        {
            m_image.transform.position = interactableObject.transform.position;
            
            m_image.enabled = true;
            m_text.enabled = true;
            m_text.text = text;
            IsDisplayed = true;
        }

        public void Hide()
        {
            m_text.text = "";
            m_image.enabled = false;
            m_text.enabled = false;
            IsDisplayed = false;
        }
    }
}
