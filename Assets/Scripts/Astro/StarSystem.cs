using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro.Bodies;
using UnityEngine;

namespace Corruption.Astro
{
    [Serializable]
    public struct OrbitParameters
    {
        public float OrbitSpeed => m_orbitSpeed;
        public AnimationCurve Radius => m_radiusCurve;
        public AnimationCurve Angle => m_angleCurve;

        [SerializeField] private float m_orbitSpeed;
        [SerializeField] private AnimationCurve m_radiusCurve;
        [SerializeField] private AnimationCurve m_angleCurve;

        public OrbitParameters(float orbitSpeed = 0.0f)
        {
            m_orbitSpeed = orbitSpeed;
            m_radiusCurve = new AnimationCurve(new Keyframe(0, 5), new Keyframe(1, 5));
            m_angleCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 360));
        }
    }

    [Serializable]
    public class SystemBody
    {
        public StellarBody Body => m_body;
        public float DistanceFromPrimary => m_distanceFromPrimary;
        public OrbitParameters OrbitParameters => m_orbitParameters;
        public float Size => m_bodyScale;
        public List<SystemBody> OrbitingBodies => m_orbitingBodies;
        
        [SerializeField] private StellarBody m_body;
        [SerializeField] private OrbitParameters m_orbitParameters;
        [SerializeField] private float m_distanceFromPrimary;
        [SerializeField] private float m_bodyScale = 1.0f;
        [SerializeField] private List<SystemBody> m_orbitingBodies;
    }
    
    [CreateAssetMenu(fileName = "New Star System", menuName = "Space/System/Star System")]
    public class StarSystem : EchoSource
    {
        public string SystemName => m_name;
        public List<SystemBody> Stars => m_stars;
        
        [SerializeField] private string m_name;
        [SerializeField] private List<SystemBody> m_stars;
        
        /// <summary> Returns the Number of System Bodies within this System </summary>
        public int GetBodyCount()
        {
            int bodyCount = 0;
            foreach (SystemBody star in m_stars)
            {
                bodyCount++;
                foreach (SystemBody orbitingBody in star.OrbitingBodies)
                {
                    bodyCount += CountBodies(orbitingBody);
                }
            }
            
            return bodyCount;
        }

        private int CountBodies(SystemBody body)
        {
            int bodyCount = 1;
            foreach (SystemBody orbitingBody in body.OrbitingBodies)
            {
                bodyCount += CountBodies(orbitingBody);
            }
            
            return bodyCount;
        }
    }
}
