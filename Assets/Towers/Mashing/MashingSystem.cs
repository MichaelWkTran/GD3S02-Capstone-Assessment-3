using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class MashingSystem : MonoBehaviour
{
    [SerializeField] Player m_player;

    public bool m_canMash = true;
#if UNITY_PS4
    [SerializeField] Vector3 m_lastGyro;
    float m_minShakeDistance = 2;
#endif
    public UnityEvent m_mashed;

    [SerializeField] Image m_mashingButtonImage;
    [SerializeField] Sprite m_pressedSprite;
    [SerializeField] Sprite m_releasedprite;
    [SerializeField] Sprite m_cantMashSprite;

    void Update()
    {
        if (m_player == null) return;

        //Gamepad gamepad = m_player.m_PlayerInput.GetDevice<Gamepad>();
        //m_player.m_PlayerInput
        //var ss = Gyroscope.current.samplingFrequency;
        bool holdButtonInput = m_player.m_interactAction.IsPressed();
        bool pressButtonInput = m_player.m_interactAction.triggered;


#if UNITY_PS4
    if (PS4Input.PadIsConnected(0))
    {
        Vector3 currentGyro = PS4Input.PadGetLastGyro(0);
        if ((currentGyro - m_lastGyro).sqrMagnitude > m_minShakeDistance * m_minShakeDistance) pressButtonInput = true;
        
        m_lastGyro = currentGyro;
    }

#endif

        //Add Mesh Score
        if (pressButtonInput && m_canMash) { m_mashed.Invoke(); }

        //Set Mash Sprite
        if (m_mashingButtonImage)
        {
            if (!m_canMash) m_mashingButtonImage.sprite = m_cantMashSprite;
            else if (holdButtonInput) m_mashingButtonImage.sprite = m_pressedSprite;
            else m_mashingButtonImage.sprite = m_releasedprite;
        }
    }
}