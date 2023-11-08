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
    public const int m_maxNumberOfPlayers = 4; //The max number of players currently in the game
    public uint m_numberOfPlayers = 1U; //The number of players currently in the game
    public InputDevice[][] m_playerInputDevices = new InputDevice[m_maxNumberOfPlayers][]; //The index of this variable represents
                                                                                           //the player index with 0 being the first player.
                                                                                           //The stored Input devices are the devices used by said player.
}