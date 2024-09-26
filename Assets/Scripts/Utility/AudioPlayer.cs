using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Utility
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> m_audioClips; // List of Audio Clips to Cycle Through

        private AudioClip m_queuedAudio = null; // Any Audio that is waiting to play is stored here
        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        public void Play()
        {
            if (m_audioClips.Count <= 0)
                return;
            
            int audioCount = m_audioClips.Count;
            int index = Random.Range(0, audioCount - 1);
            Play(index);
        }

        public void Play(int index, bool loop = false)
        {
            if (index >= 0 && index < m_audioClips.Count)
            {
                Play(m_audioClips[index], loop);
            }
        }

        public void Play(AudioClip audio, bool loop = false)
        {
            m_audioSource.clip = audio;
            m_audioSource.loop = loop;
            m_audioSource.Play();
        }

        public void Stop()
        {
            m_audioSource.Stop();
        }
    }
}
