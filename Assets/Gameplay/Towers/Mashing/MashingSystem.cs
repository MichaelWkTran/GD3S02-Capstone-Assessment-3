using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class MashingSystem : MonoBehaviour
{
    [SerializeField] float m_progressRate;
    
    public bool m_canMash = true;
#if UNITY_PS4
    [SerializeField] Vector3 m_lastGyro;
    float m_minShakeDistance = 2;
#endif

    [SerializeField] Slider m_slider;
    [SerializeField] Image m_mashingButtonImage;
    [SerializeField] Sprite m_pressedSprite;
    [SerializeField] Sprite m_releasedprite;
    [SerializeField] Sprite m_cantMashSprite;

    Tower m_tower;
    
    void Start()
    {
        m_tower = GetComponentInParent<Tower>();
    }

    void Update()
    {
        if (m_tower.m_InteractingPlayer == null) return;

        uint playerIndex = m_tower.m_InteractingPlayer.m_playerIndex;
        int gamepadSlot = GameManager.m_Current.m_playerInputIndex[playerIndex];
        
        bool holdButtonInput = (gamepadSlot < 4 && gamepadSlot > -1) ?
            InputUtilities.GetJoystickButton(gamepadSlot, 0) : 
            Input.GetKey(KeyCode.E);
        bool pressButtonInput = (gamepadSlot < 4 && gamepadSlot > -1) ?
            InputUtilities.GetJoystickButtonDown(gamepadSlot, 0) :
            Input.GetKeyDown(KeyCode.E);

#if UNITY_PS4
        if (gamepadSlot < 4 && PS4Input.PadIsConnected(gamepadSlot))
        {
            Vector3 currentGyro = PS4Input.PadGetLastGyro(gamepadSlot);
            if ((currentGyro - m_lastGyro).sqrMagnitude > m_minShakeDistance * m_minShakeDistance) pressButtonInput = true;
            
            m_lastGyro = currentGyro;
        }

#endif

        //Add Mesh Score
        if (pressButtonInput && m_canMash)
        {
            m_slider.value += m_progressRate;
            if (m_slider.value >= m_slider.maxValue) m_tower.OnCompleted();
        }

        //Set Mash Sprite
        if (m_mashingButtonImage)
        {
            if (!m_canMash) m_mashingButtonImage.sprite = m_cantMashSprite;
            else if (holdButtonInput) m_mashingButtonImage.sprite = m_pressedSprite;
            else m_mashingButtonImage.sprite = m_releasedprite;
        }
    }
}