using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    static GameManager m_current = null;
    public static GameManager m_Current
    {
        get
        {
            if (m_current == null)
            {
                m_current = new GameObject().AddComponent<GameManager>();
                m_current.gameObject.name = "Game Manager";
                DontDestroyOnLoad(m_current);
            }
            return m_current;
        }
    }

    //Inputs
    public uint m_numberOfPlayers = 1U; //The number of players currently in the game
    public List<InputDevice[]> m_playerInputDevices = new List<InputDevice[]>(); //The index of this variable represents
                                                                                 //the player index with 0 being the first player.
                                                                                 //The stored Input devices are the devices used by said player.
}