using UnityEngine;

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
                ClearPlayerInputIndex();
            }
            return m_current;
        }
    }

    //Inputs
    public const int m_maxNumberOfPlayers = 4; //The max number of players currently in the game
    public uint m_numberOfPlayers = 1U; //The number of players currently in the game
    public int[] m_playerInputIndex = new int[m_maxNumberOfPlayers]; //The index of this array represents the player index with 0 being the first player.
                                                                     //The stored values are the gamepad slot that the player is using with 0-3 representing a gamepad,
                                                                     //and 4 representing a keyboard and mouse.

    static public void ClearPlayerInputIndex()
    {
        for (int i = 0; i < m_maxNumberOfPlayers; i++) m_Current.m_playerInputIndex[i] = -1;
    }
}