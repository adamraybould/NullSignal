using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core;
using Corruption.Core.Framework;
using Corruption.Utility;
using UnityEngine;

namespace Corruption.Objects.Interactive
{
    public class Monitor : Focusable
    {
        public event Action OnTurnedOn;
        public event Action OnTurnedOff;
        
        public bool IsOn { get; private set; }

        private AudioPlayer m_audioPlayer;

        private void Awake()
        {
            m_audioPlayer = GetComponent<AudioPlayer>();
        }

        private void Start()
        {
            //m_playerCamera = Camera.main.GetComponent
            TurnOn();
        }

        public override bool Interact()
        {
            if (IsOn)
            {
                Focus();
                return true;
            }

            return false;
        }

        public void TurnOn()
        {
            IsOn = true;
            OnTurnedOn?.Invoke();
        }

        public void TurnOff()
        {
            IsOn = false;
            OnTurnedOff?.Invoke();
        }

        public void PlayAudio(AudioClip audio, bool loop = false) { m_audioPlayer.Play(audio, loop); }
        public void PlayAudio(int index, bool loop = false) { m_audioPlayer.Play(index - 1, loop); }
        public void StopAudio() { m_audioPlayer.Stop(); }
    }
}
