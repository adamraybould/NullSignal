using Corruption.Core;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.Astro
{
    public enum EchoType
    {
        STAR,
        PLANET,
        ASTEROID,
        SHIP,
        ANOMALY,
        SPECIAL,
    }

    [CreateAssetMenu(fileName = "New Echo", menuName = "Space/Echo")]
    public class Echo : ScriptableObject
    {
        public EchoSource Source => m_source;
        public EchoType EchoType => m_type;
        public SignalDifficulty DetectionDifficulty => m_difficulty;

        [SerializeField] private EchoSource m_source;          // The Source of this Echo
        [SerializeField] private EchoType m_type;               // The Type of Echo (Planet, Space Ship, etc)
        [SerializeField] private SignalDifficulty m_difficulty; // The Discovery Difficulty of this Pulse/Signal

        public float GetDifficultyMultiplier() { return SystemSettings.Instance.GetSignalDifficultyMultiplier(DetectionDifficulty); }
    }
}
