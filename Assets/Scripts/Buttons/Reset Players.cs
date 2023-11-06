using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayers : MonoBehaviour
{
	private Dictionary<int, bool> m_confirmedPlayers = new Dictionary<int, bool> ();

	private void Start ()
	{
		SetNumberOfPlayers (0);
	}

	public void SetNumberOfPlayers(int _number)
	{
		PlayerManager.instance.NumberOfPlayers = _number;

		m_confirmedPlayers.Clear ();
		for (int i = 0; i < _number; i++)
		{
			m_confirmedPlayers.Add (i, false);
		}
	}
}
