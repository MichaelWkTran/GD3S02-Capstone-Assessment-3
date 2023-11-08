using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameMode : MonoBehaviour
{
    public static GameMode m_current = null;
    [SerializeField] Player m_playerPrefab;

    public List<Player> m_players;
    public List<Tower> m_towers;

    public Vector2 m_startLocationOrigin;
    public Vector2[] m_startLocations;

    [Header("Player")]
    public Material[] m_playerMaterials;
    public Color[] m_playerColours;

    [Header("UI")]
    [SerializeField] RectTransform m_splitScreenHorizontalDivide;
    [SerializeField] RectTransform m_splitScreenVerticalDivide;
    [SerializeField] RectTransform m_winScreen;
    [SerializeField] RectTransform m_lostScreen;

    [Header("Components")]
    [SerializeField] PlayerInputManager m_playerInputManager;
    public PlayerInputManager m_PlayerInputManager { get { return m_playerInputManager; } }

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] bool m_isDebugEnabled;
    [SerializeField] bool m_isP1UsingKeyboard;
    [SerializeField] uint m_numberOfPlayers;

#endif

    void Awake()
    {
        m_current = this;
    }

    void Start()
    {
        uint numberOfPlayers = GameManager.m_Current.m_numberOfPlayers;

#if UNITY_EDITOR
        if (m_isDebugEnabled)
        {
            //Set Number of Players
            numberOfPlayers = (uint)Mathf.Min(m_numberOfPlayers, m_startLocations.Length);
        }

#endif

        //Spawn Players
        for (int playerIndex = 0; playerIndex <= numberOfPlayers; playerIndex++)
        {
            Player spawnedPlayer = Instantiate(m_playerPrefab, m_startLocationOrigin + m_startLocations[playerIndex], Quaternion.identity);
            m_players.Add(spawnedPlayer); spawnedPlayer.m_playerIndex = (uint)playerIndex;

            //Set Inputs
#if UNITY_EDITOR
            if (m_isDebugEnabled)
            {
                //Use Keyboard
                if (playerIndex == 0U && m_isP1UsingKeyboard)
                    spawnedPlayer.m_PlayerInput.SwitchCurrentControlScheme
                        (Keyboard.current, Mouse.current);
                
                //Use Gamepad
                else if (Gamepad.all.ToArray().Length > playerIndex)
                    spawnedPlayer.m_PlayerInput.SwitchCurrentControlScheme
                        (Gamepad.all[playerIndex - (m_isP1UsingKeyboard ? 1 : 0)]);
            }
            else

#endif

            spawnedPlayer.m_PlayerInput.SwitchCurrentControlScheme(GameManager.m_Current.m_playerInputDevices[playerIndex]);
                
            //Set Camera Viewport for split screen
            switch (m_numberOfPlayers)
            {
                //Two Players
                case 1:
                    m_splitScreenHorizontalDivide.gameObject.SetActive(true);
                    switch (playerIndex)
                    {
                        case 0: spawnedPlayer.m_Camera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f); break;
                        case 1: spawnedPlayer.m_Camera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f); break;
                    }
                    break;

                //Three or Four Players
                case 2:
                case 3:
                    m_splitScreenHorizontalDivide.gameObject.SetActive(true);
                    m_splitScreenVerticalDivide.gameObject.SetActive(true);
                    switch (playerIndex)
                    {
                        case 0: spawnedPlayer.m_Camera.rect = new Rect(0.0f, 0.5f, 0.5f, 0.5f); break;
                        case 1: spawnedPlayer.m_Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); break;
                        case 2: spawnedPlayer.m_Camera.rect = new Rect(0.0f, 0.0f, 0.5f, 0.5f); break;
                        case 3: spawnedPlayer.m_Camera.rect = new Rect(0.5f, 0.0f, 0.5f, 0.5f); break;
                    }
                    break;
            }
        }

        //Get Towers
        m_towers = new List<Tower>(FindObjectsByType<Tower>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    public void OnPlayerKilled(Player _player)
    {
        //Don't trigger if game mode is disabled
        if (!enabled) return;

        //Remove player from list
        m_players.Remove(_player);

        //Check whether there are no players remaining
        if (m_players.Count > 0) return;

        //Show Lost Screen
        enabled = false;
        m_lostScreen.gameObject.SetActive(true);
    }

    public void OnTowerCompleted(Tower _tower)
    {
        //Don't trigger if game mode is disabled
        if (!enabled) return;

        //Remove tower from list
        m_towers.Remove(_tower);

        //Check whether there are no players remaining
        if (m_towers.Count > 0) return;

        //Show Win Screen
        enabled = false;
        m_winScreen.gameObject.SetActive(true);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameMode))]
public class GameModeEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        GameMode gameMode = target as GameMode;

        //Set Start Location Origin
        gameMode.m_startLocationOrigin = (Vector2)Handles.PositionHandle((Vector3)gameMode.m_startLocationOrigin, Quaternion.identity);
        
        //Set Start Locations
        for (int i = 0; i < gameMode.m_startLocations.Length; i++)
        {
            gameMode.m_startLocations[i] = (Vector2)Handles.PositionHandle
                ((Vector3)(gameMode.m_startLocations[i] + gameMode.m_startLocationOrigin), Quaternion.identity);
            
            gameMode.m_startLocations[i] -= gameMode.m_startLocationOrigin;
        }
    }
}

#endif