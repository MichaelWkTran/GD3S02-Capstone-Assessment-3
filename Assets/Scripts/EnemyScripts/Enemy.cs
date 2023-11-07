using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int m_health;
    [SerializeField] private int m_maxHealth;
    [SerializeField] private AIBehaviour m_thisBehaviour;
    [SerializeField] private int m_attackPower;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            m_thisBehaviour.ChangeState(aiState.moving);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            m_thisBehaviour.ChangeState(aiState.idle);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_thisBehaviour.ChangeState(aiState.attacking);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            m_thisBehaviour.ChangeState(aiState.dead);
        }
    }
}
