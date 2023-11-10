using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        m_eventSystem = FindAnyObjectByType<EventSystem>();

        #region Device Assign Screen
        //Setup Device Slot
        for (int i = 0; i < GameManager.m_maxNumberOfPlayers; i++) m_playerDeviceSlots[i].m_playerIndex = (uint)i;

        //Automatically set player 1 to use keyboard
        if (Keyboard.current != null && Mouse.current != null && GameManager.m_Current.m_playerInputDevices[0] == null)
        {
            GameManager.m_Current.m_playerInputDevices[0] = new InputDevice[] { Keyboard.current, Mouse.current };
            m_playerDeviceSlots[0].m_IsControllerAssigned = true;
        }

        #endregion
    }

    void Update()
    {
        if (m_deviceAssignScreen.gameObject.activeSelf)
        {
            foreach (Gamepad gamepad in Gamepad.all)
            {
                //Exit Device Assign Screen when pressing the cancel button on the gamepad
                if (gamepad.buttonEast.wasPressedThisFrame) m_deviceAssignBackButton.onClick.Invoke();
            }
            
            //Detect new controllers to assign to players
            DetectPlayerControllers();
        }
    }

    #region Select Num Players
    public void ConfirmNumPlayersOnClick(int _numPlayers)
    {
        //Set Number of Players
        GameManager.m_Current.m_numberOfPlayers = (uint)_numPlayers;
        GameManager.m_Current.m_playerInputDevices = new InputDevice[GameManager.m_maxNumberOfPlayers][];
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
        InputDevice[][] playerInputDevices = GameManager.m_Current.m_playerInputDevices;

        //Check whether there are any empty slots avalible
        {
            int availableIndex = Array.FindIndex(playerInputDevices, i => i == null);
            if (availableIndex < 0 || availableIndex > GameManager.m_Current.m_numberOfPlayers) return;
        }

        //Search all Gamepads
        foreach (Gamepad gamepad in Gamepad.all)
        {
            //Check whether there are any inputs detected from the device
            if (!gamepad.startButton.wasPressedThisFrame) continue;

            //Check whether the input device is not already recorded
            InputDevice[] foundInputDevices = Array.Find(playerInputDevices, i => { return (i == null) ? false : i.Contains(gamepad); });
            if (foundInputDevices != null) continue;

            //Check whether there are any avalible slots for the new controller
            int availableIndex = Array.FindIndex(playerInputDevices, i => i == null);
            if (availableIndex < 0) continue;

            //Record input device
            playerInputDevices[availableIndex] = new InputDevice[] { gamepad };
            m_playerDeviceSlots[availableIndex].m_IsControllerAssigned = true;
        }
    }

    public void EnableReadyButton()
    {
        //Show ready button when all players have a controller assigned
        int availableIndex = Array.FindIndex(GameManager.m_Current.m_playerInputDevices, i => i == null);
        m_readyButton.interactable = availableIndex > GameManager.m_Current.m_numberOfPlayers || availableIndex < 0;
        
        //Select the ready button
        if (m_readyButton.interactable) m_eventSystem.SetSelectedGameObject(m_readyButton.gameObject);
    }

    #region P1 Only Methods
    void P1AssignKeyboard()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        InputDevice[][] playerInputDevices = GameManager.m_Current.m_playerInputDevices;
        playerInputDevices[0] = new InputDevice[] { Keyboard.current, Mouse.current };
        m_playerDeviceSlots[0].m_IsControllerAssigned = true;
        m_playerDeviceSlots[0].SetKeyboardIcon();
    }

    public void P1SwapKeyboardGamepad()
    {
        InputDevice[][] playerInputDevices = GameManager.m_Current.m_playerInputDevices;

        //Swap to gamepad
        if (playerInputDevices[0] != null && Array.Find(playerInputDevices[0], i => i as Keyboard != null) != null)
        {
            playerInputDevices[0] = null;
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
