using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corruption.UI
{
    public abstract class UIWindow : MonoBehaviour
    {
        public UICanvas Canvas { get; private set; }
        protected Animator m_animator { get; private set; }

        protected virtual void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void SetCanvas(UICanvas canvas) { Canvas = canvas; }
        public void SetActive(bool value) { gameObject.SetActive(value); }
        
        protected void PlayAudio(AudioClip audio, bool loop = false) { Canvas.PlayAudio(audio, loop);}
        protected void PlayAudio(int index, bool loop = false) { Canvas.PlayAudio(index, loop); }
        protected void StopAudio() { Canvas.StopAudio(); }
    }
}
