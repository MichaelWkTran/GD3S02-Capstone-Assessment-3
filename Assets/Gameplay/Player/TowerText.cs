using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerText : MonoBehaviour
{
	private TMP_Text m_tower;
	private int m_maxTowers;

	private void Start ()
	{
		m_tower = GetComponent<TMP_Text> ();

		m_maxTowers = GameMode.m_current.m_towers.Capacity;

		string output = $"0 / {m_maxTowers}";

		m_tower.text = output;

		GameMode.m_current.m_updateUI += DisplayText;
	}

	private void DisplayText()
	{
		string output = $"{GameMode.m_current.m_towers.Capacity} / {m_maxTowers}";

		m_tower.text = output;
	}

	private void OnDestroy ()
	{
		GameMode.m_current.m_updateUI -= DisplayText;
	}
}
