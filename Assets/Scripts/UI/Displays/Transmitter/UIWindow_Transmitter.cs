using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Core;
using Corruption.Objects;
using TMPro;
using UnityEngine;

namespace Corruption.UI.Display
{
    public class UIWindow_Transmitter : UIWindow
    {
        [Header("Transmitter")] 
        [SerializeField] private TextMeshProUGUI m_credits;
        [SerializeField] private List<UIWidget_SavedSignal> m_savedSignalWidgets;
        
        private void Start()
        {
            SignalManager.Instance.OnSignalSaved += OnSignalSaved;
            SignalManager.Instance.OnSignalDeleted += OnSignalDeleted;
            Economy.Instance.OnSold += OnSold;
        }

        private void OnSignalSaved(Signal signal)
        {
            UIWidget_SavedSignal availableWidget = GetAvailableSignalWidget();
            availableWidget.SetWidget(signal.StellarBody.Name);
        }

        private void OnSignalDeleted(Signal signal)
        {
            foreach (UIWidget_SavedSignal widget in m_savedSignalWidgets)
            {
                if (signal.GetID() == widget.SignalID)
                {
                    widget.RemoveWidget();
                    m_savedSignalWidgets.Remove(widget);
                    return;
                }
            }
        }
        
        private void OnSold(int credits)
        {
            m_credits.text = credits.ToString();
            
            m_animator.SetTrigger("Signal_Sold");
            PlayAudio(1);
        }

        private UIWidget_SavedSignal GetAvailableSignalWidget()
        {
            foreach (UIWidget_SavedSignal widget in m_savedSignalWidgets)
            {
                if (widget.gameObject.activeInHierarchy)
                    continue;

                return widget;
            }

            Debug.LogError("No Available UI Saved Signal Widgets");
            return null;
        }
    }
}
