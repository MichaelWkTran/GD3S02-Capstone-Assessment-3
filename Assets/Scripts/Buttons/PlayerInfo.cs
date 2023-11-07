using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	[SerializeField] private int m_playerIndex = 0;

	public int PlayerIndex => m_playerIndex;
}
