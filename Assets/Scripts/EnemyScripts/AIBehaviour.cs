using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField] private aiState m_currentState;
    [SerializeField] private AIState m_actingState;
    
    private IdleState m_idleState;
    private MoveState m_movingState;
    private AttackState m_attackState;
    private DeadState m_deadState;
    
    public void ChangeState(aiState _newState)
    {
        m_actingState.ExitState();
        m_currentState = _newState;
        switch (_newState)
        {
            case aiState.idle:
                m_actingState = m_idleState;
                break;
            
            case aiState.moving:
                m_actingState = m_movingState;
                break;
            
            case aiState.attacking:
                m_actingState = m_attackState;
                break;
            
            case aiState.dead:
                m_actingState = m_deadState;
                break;
        }
        m_actingState.EnterState();
    }
    void Start()
    {
        m_idleState = new IdleState(this);
        m_movingState = new MoveState(this);
        m_attackState = new AttackState(this);
        m_deadState = new DeadState(this);
        ChangeState(aiState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        m_actingState.UpdateState();
    }
}
public enum aiState
{
    idle,
    moving,
    attacking,
    dead,
}


