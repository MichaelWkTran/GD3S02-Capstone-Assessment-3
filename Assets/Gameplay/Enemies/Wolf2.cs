using UnityEngine;
using Pathfinding;

public class Wolf2 : Enemy2
{
    [SerializeField] float m_speed;
    [SerializeField] float m_damage;

    [Header("AI")]
    [SerializeField] Seeker m_seeker;
    [SerializeField] float nextWaypointDistance = 3.0f;
    Player m_target;
    Path m_path;
    int m_currentWaypoint = 0;
    public bool m_reachedEndOfPath { get; private set; } = false;
    public bool canAttack;
    float attackTimer = 1;

    [Header("Components")]
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Animator m_animator;
    [SerializeField] Rigidbody2D m_rigidbody;
    
    void Start()
    {
        base.Start();
        InvokeRepeating("UpdatePath", 0.0f, 0.5f);
    }

    void UpdatePath()
    {
        //Get the closest player to the enemy
        {
            float currentDistance = float.PositiveInfinity;
            foreach (Player player in FindObjectsOfType<Player>(false))
            {
                if (!player.enabled) continue;

                float distance = Vector2.SqrMagnitude(transform.position - player.transform.position);
                if (distance >= currentDistance) continue;

                currentDistance = distance;
                m_target = player;
            }
        }

        //Check whether the target is valid
        if (m_target == null) return;

        //Update the path
        if (m_seeker.IsDone()) m_seeker.StartPath(m_rigidbody.position, m_target.transform.position, OnPathComplete);
    }

    void OnPathComplete(Path _path)
    {
        if (_path.error) return;
        
        m_path = _path;
        m_currentWaypoint = 0;
    }

    private void Update()
    {
        ResetAttackTimer();
    }

    void FixedUpdate()
    {
        if (m_path == null) return;

        //Move towards target
        if (m_target)
        {
            //Clamp Current Waypoint
            m_currentWaypoint = Mathf.Clamp(m_currentWaypoint, 0, m_path.vectorPath.Count - 1);
            
            //Move Enemy
            Vector2 direction = ((Vector2)m_path.vectorPath[m_currentWaypoint] - m_rigidbody.position).normalized;
            Vector2 force = direction * m_speed * m_rigidbody.mass / ((1.0f / m_rigidbody.drag) - Time.fixedDeltaTime);
            m_rigidbody.AddForce(force, ForceMode2D.Force);

            //Update Waypoint
            if (Vector2.SqrMagnitude(m_rigidbody.position - (Vector2)m_path.vectorPath[m_currentWaypoint]) < nextWaypointDistance * nextWaypointDistance)
                m_currentWaypoint++;

            //Updated Reached End of Path
            if (m_currentWaypoint >= m_path.vectorPath.Count) m_reachedEndOfPath = true;
            else m_reachedEndOfPath = false;
        }
    }

    void LateUpdate()
    {
        m_spriteRenderer.flipX = m_rigidbody.velocity.x > 0.0f;
        m_animator.SetFloat("Speed", m_rigidbody.velocity.sqrMagnitude);
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.tag == "Player Projectile")
        {
            //Damage Enemy
            m_Health -= 1.0f;
            SoundManager.Instance.PlaySound(6, 0.5f, gameObject, false, false);

            //Sprite Flash
            LeanTween.value(gameObject, 0.0f, 1.0f, 0.5f).setEasePunch()
                .setOnUpdate((float _flashAlpha) => { m_spriteRenderer.material.SetFloat("_FlashAlpha", _flashAlpha); });

            //Destroy Projectile
            Destroy(_col.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D _col)
    {
        //Damage Player
        {
            Player player = _col.gameObject.GetComponent<Player>();
            if (player != null && canAttack)
            {
                player.OnDamage(m_damage);
                SoundManager.Instance.PlaySound(5, 0.5f, gameObject, false, false);
                canAttack = false;
            }
        }
    }


    void ResetAttackTimer()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            canAttack = true;
            attackTimer = 1;
        }
    }

    public override void Kill()
    {
        if (!enabled) return;
        
        m_animator.SetTrigger("Kill");
        Destroy(gameObject, 1.0f);
        enabled = false;
        SoundManager.Instance.PlaySound(7, 0.5f, gameObject, false, false);
    }
}
