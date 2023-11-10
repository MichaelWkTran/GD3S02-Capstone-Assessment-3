using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

public class PlayerDeviceSlot : MonoBehaviour
{
    bool m_isControllerAssigned = false;
    public bool m_IsControllerAssigned
    {
        get { return m_isControllerAssigned; }
        set
        {
            m_isControllerAssigned = value;
            m_assignedUI.gameObject.SetActive(m_isControllerAssigned);
            m_unassignedUI.gameObject.SetActive(!m_isControllerAssigned);
            FindAnyObjectByType<PlayerSelectScreen>().EnableReadyButton();
        }
    }
    public uint m_playerIndex = 0U;

    public void SetControllerIcon()
    {
    m_iconImage.sprite = m_controllerIcon;
    }

    public void SetKeyboardIcon()
    {
    m_iconImage.sprite = m_keyboardIcon;
    }
    
    [SerializeField] RectTransform m_assignedUI;
    [SerializeField] RectTransform m_unassignedUI;
    [SerializeField] Sprite m_controllerIcon;
    [SerializeField] Sprite m_keyboardIcon;
    [SerializeField] Image m_iconImage;
}
