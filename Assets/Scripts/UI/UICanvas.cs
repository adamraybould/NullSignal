using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Utility;
using UnityEngine;

namespace Corruption.UI
{
    [RequireComponent(typeof(Canvas))]
    public class UICanvas : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private List<UIWindow> m_elements;
        
        private AudioPlayer m_audioPlayer;
        private bool m_hasAudio;

        protected virtual void Awake()
        {
            foreach (UIWindow element in m_elements)
            {
                element.SetCanvas(this);
            }

            m_audioPlayer = GetComponent<AudioPlayer>();
            if (m_audioPlayer)
            {
                m_hasAudio = true;
            }
        }

        public void ActivateElement(int index)
        {
            if (index < 0 || index >= m_elements.Count)
            {
                Debug.LogError("Trying to Element with Out of Bounds Index (Index: " + index + ")");
                return;
            }

            for (int i = 0; i < m_elements.Count; i++)
            {
                UIWindow window = m_elements[i];
                if (i == index)
                    window.SetActive(true);
                else
                    window.SetActive(false);
            }
        }
        
        public T GetUIElement<T>() where T : UIWindow
        {
            foreach (UIWindow element in m_elements)
            {
                if (element is T)
                {
                    return element as T;
                }
            }

            Debug.LogError("No UI Element found of type '" + typeof(T) + "'");
            return null;
        }
        
        public void SetActive(bool value) { gameObject.SetActive(value); }
        
        public void PlayAudio(AudioClip audio, bool loop = false) { if (m_hasAudio) m_audioPlayer?.Play(audio, loop); }
        public void PlayAudio(int index, bool loop = false) { if (m_hasAudio) m_audioPlayer?.Play(index - 1, loop); }
        public void StopAudio() { if(m_hasAudio) m_audioPlayer?.Stop(); }
    }
}
