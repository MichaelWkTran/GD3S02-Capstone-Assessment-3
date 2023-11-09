using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        }
    }
    public uint m_playerIndex = 0U;
    
    [SerializeField] RectTransform m_assignedUI;
    [SerializeField] RectTransform m_unassignedUI;

}
