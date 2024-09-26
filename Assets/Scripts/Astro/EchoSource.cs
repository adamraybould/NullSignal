using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Corruption.Astro
{
    public class EchoBaseCode
    {
        public int FirstDigit;
        public int SecondDigit;

        private int m_positionIndex; // Used to determine where the First and Second Digts will be in the Base Code
        private int[] m_baseCodeArray;
        
        public EchoBaseCode()
        {
            FirstDigit = Random.Range(0, 9);
            SecondDigit = Random.Range(0, 9);
            m_positionIndex = Random.Range(0, 7);
            
            m_baseCodeArray = m_baseCodeArray = new int[8];
            for (int i = 0; i < m_baseCodeArray.Length; i++)
            {
                m_baseCodeArray[i] = Random.Range(0, 9);
            }
        }
    }
    
    public class EchoSource : ScriptableObject
    {
        public int Distance => m_sourceDistance;
        public float Size => m_sourceSize;
        public Color Color => m_sourceColor;
        
        [Header("Echo Source Properties")] 
        [SerializeField] private int m_sourceDistance = 1;
        [SerializeField] private float m_sourceSize = 1.0f;
        [SerializeField] private Color m_sourceColor = Color.white;
    }
}
