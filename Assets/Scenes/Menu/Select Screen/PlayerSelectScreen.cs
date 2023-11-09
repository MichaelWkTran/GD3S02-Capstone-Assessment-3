using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerSelectScreen : MonoBehaviour
{
	[SerializeField] RectTransform m_selectNumPlayersScreen;
	[SerializeField] RectTransform m_readyScreen;

	[Header ("Device Assign Screen")]
	[SerializeField] RectTransform m_deviceAssignScreen;
	[SerializeField] PlayerDeviceSlot [] m_playerDeviceSlots;

	void Start ()
	{
		#region Device Assign Screen
		//Setup Device Slot
		for (int i = 0; i < GameManager.m_maxNumberOfPlayers; i++)
			m_playerDeviceSlots [i].m_playerIndex = (uint)i;

		//Automatically set player 1 to use keyboard
		if (Keyboard.current != null && Mouse.current != null && GameManager.m_Current.m_playerInputDevices [0] == null)
		{
			GameManager.m_Current.m_playerInputDevices [0] = new InputDevice [] { Keyboard.current, Mouse.current };
			m_playerDeviceSlots [0].m_IsControllerAssigned = true;
		}

		#endregion
	}

	void Update ()
	{
		CheckAllPlayersReady ();

		//Detect new controllers to assign to players
		DetectPlayerControllers ();
	}

	#region Select Num Players
	public void ConfirmNumPlayersOnClick (int _numPlayers)
	{
		//Set Number of Players
		GameManager.m_Current.m_numberOfPlayers = (uint)_numPlayers;
		GameManager.m_Current.m_playerInputDevices = new InputDevice [GameManager.m_maxNumberOfPlayers] [];
		foreach (PlayerDeviceSlot deviceSlot in m_playerDeviceSlots)
			deviceSlot.m_IsControllerAssigned = false;

		//Show and Hide Screens
		m_selectNumPlayersScreen.gameObject.SetActive (false);
		m_deviceAssignScreen.gameObject.SetActive (true);

		//Show Necessary Slots
		for (int i = 0; i < m_playerDeviceSlots.Length; i++)
			m_playerDeviceSlots [i].gameObject.SetActive (i <= _numPlayers);
	}

	#endregion

	#region Device Assign Screen
	public void DetectPlayerControllers ()
	{
		InputDevice [] [] playerInputDevices = GameManager.m_Current.m_playerInputDevices;

		//Check whether there are any empty slots avalible
		{
			int availableIndex = ArrayUtility.FindIndex (playerInputDevices, i => i == null);
			if (availableIndex < 0 || availableIndex >= GameManager.m_maxNumberOfPlayers)
				return;
		}

		//Search all Gamepads
		foreach (Gamepad gamepad in Gamepad.all)
		{
			//Check whether there are any inputs detected from the device
			if (!(
					gamepad.buttonNorth.wasPressedThisFrame ||
					gamepad.buttonEast.wasPressedThisFrame ||
					gamepad.buttonSouth.wasPressedThisFrame ||
					gamepad.buttonWest.wasPressedThisFrame ||
					gamepad.startButton.wasPressedThisFrame
					))
				continue;

			//Check whether the input device is not already recorded
			InputDevice [] foundInputDevices = ArrayUtility.Find (playerInputDevices, i => { return (i == null) ? false : i.Contains (gamepad); });
			if (foundInputDevices != null)
				continue;

			//Check whether there are any avalible slots for the new controller
			int availableIndex = ArrayUtility.FindIndex (playerInputDevices, i => i == null);
			if (availableIndex < 0)
				continue;

			//Record input device
			playerInputDevices [availableIndex] = new InputDevice [] { gamepad };
			m_playerDeviceSlots [availableIndex].m_IsControllerAssigned = true;
		}
	}

	public void CheckAllPlayersReady ()
	{
		if (!m_deviceAssignScreen.gameObject.activeSelf)
			return;

		for (int i = 0; i < m_playerDeviceSlots.Length; i++)
		{
			if (i > 0 && !m_playerDeviceSlots [i].m_IsControllerAssigned && m_playerDeviceSlots [i].gameObject.activeSelf)
			{
				return;
			}

			else if (!m_playerDeviceSlots [i].m_IsControllerAssigned && m_playerDeviceSlots [i].gameObject.activeSelf)
			{
				return;
			}
		}

		m_readyScreen.gameObject.SetActive (true);

		EventSystem eventSystem = FindObjectOfType<EventSystem> ();
		eventSystem.SetSelectedGameObject (m_readyScreen.GetChild (0).gameObject);
	}

	#region P1 Only Methods
	public void P1SwapKeyboardGamepad ()
	{
		InputDevice [] [] playerInputDevices = GameManager.m_Current.m_playerInputDevices;

		//Swap to gamepad
		if (playerInputDevices [0] != null && ArrayUtility.Find (playerInputDevices [0], i => Keyboard.current != null) != null)
		{
			playerInputDevices [0] = null;
			m_playerDeviceSlots [0].m_IsControllerAssigned = false;
		}
		//Swap to keyboard
		else
		{
			playerInputDevices [0] = new InputDevice [] { Keyboard.current, Mouse.current };
			m_playerDeviceSlots [0].m_IsControllerAssigned = true;
		}
	}

	#endregion

	#endregion

}
