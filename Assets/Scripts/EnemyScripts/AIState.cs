using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIState
{
    protected Enemy m_thisEnemy;
    protected AIBehaviour m_thisBehaviour;
    protected aiState m_stateEnum;

    public AIState(AIBehaviour _behaviour, aiState _state)
    {
        m_thisBehaviour = _behaviour;
        m_stateEnum = _state;
    }
    
    public virtual void EnterState()
    {
        
    }

    public virtual void UpdateState()
    {
        
    }
    
    public virtual void ExitState()
    {
        
    }
}

[System.Serializable]
public class IdleState : AIState
{
    public IdleState(AIBehaviour _behaviour) : base(_behaviour, aiState.idle)
    {
        
    }
    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        m_thisBehaviour.gameObject.transform.position += new Vector3(0.2f, 0, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
        m_thisBehaviour.GetComponent<SpriteRenderer>().color = Color.blue;

    }
}
[System.Serializable]
public class MoveState : AIState
{
    public MoveState(AIBehaviour _behaviour) : base(_behaviour, aiState.moving)
    {
        
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        m_thisBehaviour.gameObject.transform.position += new Vector3(0, 0.2f, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
        m_thisBehaviour.GetComponent<SpriteRenderer>().color = Color.green;

    }
}
[System.Serializable]
public class AttackState : AIState
{
    
    public AttackState(AIBehaviour _behaviour) : base(_behaviour, aiState.attacking)
    {
    
    }
    
    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        m_thisBehaviour.gameObject.transform.position += new Vector3(0, -0.2f, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
        m_thisBehaviour.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
[System.Serializable]
public class DeadState : AIState
{
    public DeadState(AIBehaviour _behaviour) : base(_behaviour, aiState.dead)
    {
    
    }
    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
        m_thisBehaviour.gameObject.transform.position += new Vector3(-0.2f, 0, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
        GameObject.Destroy(m_thisBehaviour.gameObject);

    }
}