using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Objects.Interactive
{
    public class Button : InteractiveObject
    {
        public bool Pressed { get; private set; }
        
        private AudioSource m_audioSource;
        private Animator m_animator;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            m_animator = GetComponent<Animator>();
        }

        public override bool Interact()
        {
            if (Pressed)
                return false;
            
            Pressed = true;
            m_audioSource.Play();
            m_animator.SetTrigger("Pressed");
            
            return base.Interact();
        }

        public void Anim_OnAnimationEnded()
        {
            Pressed = false;
        }
    }
}
