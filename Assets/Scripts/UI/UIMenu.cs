using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEngine;

namespace Corruption.UI.Menus
{
    public abstract class UIMenu : UICanvas
    {
        public event Action<MenuID> OnMenuOpen;
        public event Action<MenuID> OnMenuClose;

        public MenuID ID => m_menuID;
        public bool IsOpen { get; private set; }

        [SerializeField] private MenuID m_menuID;

        protected virtual void Start()
        {
            if (gameObject.activeInHierarchy)
            {
                OpenMenu();
            }
        }

        public virtual void OpenMenu()
        {
            IsOpen = true;
            Cursor.lockState = CursorLockMode.Confined;
            gameObject.SetActive(true);
            
            OnMenuOpen?.Invoke(ID);
        }

        public virtual void CloseMenu()
        {
            IsOpen = false;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(false);
            
            OnMenuClose?.Invoke(ID);
        }
    }
}
