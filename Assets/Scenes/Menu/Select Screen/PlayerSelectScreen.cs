using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSelectScreen : MonoBehaviour
{
    [SerializeField] RectTransform m_selectNumPlayersScreen;

    [Header("Device Assign Screen")]
    [SerializeField] RectTransform m_deviceAssignScreen;
    [SerializeField] PlayerDeviceSlot[] m_playerDeviceSlots;
    [SerializeField] UnityEngine.UI.Button m_deviceAssignBackButton;
    [SerializeField] UnityEngine.UI.Button m_readyButton;

    EventSystem m_eventSystem;

    void Start()
    {
        m_eventSystem = FindObjectOfType<EventSystem>();

        #region Device Assign Screen
        //Setup Device Slot
        for (int i = 0; i < GameManager.m_maxNumberOfPlayers; i++) m_playerDeviceSlots[i].m_playerIndex = (uint)i;

        //Automatically set player 1 to use keyboard
        if (GameManager.m_Current.m_playerInputIndex[0] == -1)
        {
            GameManager.m_Current.m_playerInputIndex[0] = 4;
            m_playerDeviceSlots[0].m_IsControllerAssigned = true;
        }

        #endregion
    }

    void Update()
    {
        if (m_deviceAssignScreen.gameObject.activeSelf)
        {
            //Exit Device Assign Screen when pressing the cancel button on the gamepad
            if (Input.GetKeyDown("joystick button 1")) m_deviceAssignBackButton.onClick.Invoke();
            
            //Detect new controllers to assign to players
            DetectPlayerControllers();
        }
    }

    #region Select Num Players
    public void ConfirmNumPlayersOnClick(int _numPlayers)
    {
        //Set Number of Players
        GameManager.m_Current.m_numberOfPlayers = (uint)_numPlayers;
        GameManager.ClearPlayerInputIndex();
        foreach (PlayerDeviceSlot deviceSlot in m_playerDeviceSlots) deviceSlot.m_IsControllerAssigned = false;

        //Show and Hide Screens
        m_selectNumPlayersScreen.gameObject.SetActive(false);
        m_deviceAssignScreen.gameObject.SetActive(true);

        //Show Necessary Slots
        for (int i = 0; i < m_playerDeviceSlots.Length; i++) m_playerDeviceSlots[i].gameObject.SetActive(i <= _numPlayers);

        //Disable Confirm Button
        m_readyButton.interactable = false;

        //Set P1 to Keyboard
        P1AssignKeyboard();
    }

    #endregion

    #region Device Assign Screen
    public void DetectPlayerControllers()
    {
        int[] playerInputDevices = GameManager.m_Current.m_playerInputIndex;

        //Check whether there are any empty slots avalible
        //if (!playerInputDevices.Contains(-1)) return;
        {
            int availableIndex = Array.FindIndex(playerInputDevices, i => i == -1);
            if (availableIndex < 0 || availableIndex > GameManager.m_Current.m_numberOfPlayers) return;
        }
        

        //Search all Gamepads
        for (int gamepadIndex = 0; gamepadIndex < 4; gamepadIndex++)
        {
            //Check whether the player pressed the start button of the gamepad
            if (!InputUtilities.GetJoystickButton(gamepadIndex, 7)) continue;
            //Check whether the input device is not already recorded
            if (playerInputDevices.Contains(gamepadIndex)) continue;

            //Check whether there are any avalible slots for the new controller
            int availableIndex = Array.FindIndex(playerInputDevices, i => i == -1);
            if (availableIndex < 0) continue;

            //Record input device
            playerInputDevices[availableIndex] = gamepadIndex;
            m_playerDeviceSlots[availableIndex].m_IsControllerAssigned = true;
        }
    }

    public void EnableReadyButton()
    {
        //Show ready button when all players have a controller assigned
        int availableIndex = Array.FindIndex(GameManager.m_Current.m_playerInputIndex, i => i == -1);
        m_readyButton.interactable = availableIndex > GameManager.m_Current.m_numberOfPlayers || availableIndex < 0;
        
        //Select the ready button
        if (m_readyButton.interactable) m_eventSystem.SetSelectedGameObject(m_readyButton.gameObject);
    }

    #region P1 Only Methods
    void P1AssignKeyboard()
    {
        int[] playerInputDevices = GameManager.m_Current.m_playerInputIndex;
        playerInputDevices[0] = 4;
        m_playerDeviceSlots[0].m_IsControllerAssigned = true;
        m_playerDeviceSlots[0].SetKeyboardIcon();
    }

    public void P1SwapKeyboardGamepad()
    {
        int[] playerInputDevices = GameManager.m_Current.m_playerInputIndex;

        //Swap to gamepad
        if (playerInputDevices[0] >= 4)
        {
            playerInputDevices[0] = -1;
            m_playerDeviceSlots[0].m_IsControllerAssigned = false;
            m_playerDeviceSlots[0].SetControllerIcon();
        }
        //Swap to keyboard
        else
        {
            P1AssignKeyboard();
        }
    }

    #endregion

    #endregion

}
