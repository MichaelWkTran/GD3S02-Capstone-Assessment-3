using UnityEngine;
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
