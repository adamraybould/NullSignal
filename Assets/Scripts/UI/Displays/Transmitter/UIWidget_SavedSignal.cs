using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Corruption.UI.Display
{
    public class UIWidget_SavedSignal : MonoBehaviour
    {
        public string SignalID { get; private set; }
        
        [SerializeField] private TextMeshProUGUI m_IDText;

        private void Awake()
        {
            RemoveWidget();
        }

        public void SetWidget(string ID)
        {
            SignalID = ID;
            m_IDText.text = SignalID;
            gameObject.SetActive(true);
        }

        public void RemoveWidget()
        {
            SignalID = "";
            m_IDText.text = SignalID;
            gameObject.SetActive(false);
        }
    }
}
