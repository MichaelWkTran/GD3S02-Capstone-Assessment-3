using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
	public void StartGame()
	{
		SceneManager.LoadScene ("Select Screen");
	}

	public void BackToMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame()
	{

	}
}
