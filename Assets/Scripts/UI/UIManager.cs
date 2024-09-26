using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using Corruption.UI.Menus;
using UnityEngine;

namespace Corruption.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public UIMenu CurrentMenu { get; private set; }

        [SerializeField] private UIPlayerHUD m_playerHUD;
        
        [Space, SerializeField] private List<UIMenu> m_menuList; // List of all Menus in the Game. Is used to be able to set Menus in the Inspector
        private Dictionary<int, UIMenu> m_menus; // List of Menus with ID's

        protected override void Awake()
        {
            base.Awake();

            m_menus = new Dictionary<int, UIMenu>();
            foreach (UIMenu menu in m_menuList)
            {
                int ID = (int)menu.ID;
                m_menus.Add(ID, menu);
            }
        }

        private void Start()
        {
            GameEvents.MenuRequest.Event += OpenMenu;
            
            foreach (KeyValuePair<int, UIMenu> menu in m_menus)
            {
                menu.Value.OnMenuClose += OnMenuClosed;
            }
        }
        
        public void OpenMenu(MenuID menuID)
        {
            int ID = (int)menuID;
            if (m_menus.ContainsKey(ID))
            {
                CurrentMenu = m_menus[ID];
                
                CurrentMenu.OpenMenu();
                m_playerHUD.LockMouse(true);
                
                GameEvents.OnMenuOpened.Trigger(menuID);
                return;
            }
            
            Debug.LogError("Unknown Menu has Opened with ID '" + menuID + "'");
        }
        
        private void OnMenuClosed(MenuID menuID)
        {
            CurrentMenu = null;
            m_playerHUD.LockMouse(false);
            
            GameEvents.OnMenuClosed.Trigger(menuID);
        }
    }
}
