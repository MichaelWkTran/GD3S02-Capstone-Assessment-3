using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	public void StartGame()
	{

	}

	public void BackToMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame()
	{

	}
}
