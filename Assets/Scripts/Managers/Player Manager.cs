using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance { get; private set; }

	private int numOfPlayers = 1;

	public int NumberOfPlayers
	{
		set => numOfPlayers = value;
		get => numOfPlayers;
	}

	public void DisplayNumberOfPlayers()
	{
		Debug.Log (numOfPlayers);
	}

	private void Awake ()
	{
		if (instance != null && instance != this)
		{
			Destroy (this.gameObject);
		}

		else
		{
			instance = this;
		}

		DontDestroyOnLoad (this);
	}
}
