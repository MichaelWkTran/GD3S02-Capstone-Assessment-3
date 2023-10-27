using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Structs, Classes, and Enum
    public enum PlayerState { Default, TowerRestoring }

    #endregion

    [SerializeField] float m_speed;

    [Header("Projectile")]
    [SerializeField] Rigidbody2D m_projectile;
    [SerializeField] float m_projectileSpeed;
    [SerializeField] float m_projectileLifetime;
    [SerializeField] float m_projectileYOffset;

    [Header("Game Mode")]
    public uint m_playerIndex;
    float m_maxHealth = 100.0f;
    float m_health = 100.0f;
    PlayerState m_state;
    public float m_Health
    {
        get {  return m_health; }
        set
        {
            m_health = Mathf.Clamp(value, 0.0f, m_maxHealth);
            if (m_health <= 0.0f) Kill();
        }
    }

    [Header("Inputs")]
    [SerializeField] PlayerInput m_playerInput; public PlayerInput m_PlayerInput { get { return m_playerInput; } }
    InputAction m_moveAction;
    InputAction m_shootVectorAction;
    InputAction m_shootButtonAction;
    InputAction m_interactAction;
    InputAction m_cancelAction;
    Vector2 m_shootVector;

    [Header("Components")]
    [SerializeField] Camera m_camera; public Camera m_Camera { get { return m_camera; } }
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Animator m_animator;
    [SerializeField] Rigidbody2D m_rigidbody;

    void Start()
    {
        m_moveAction = m_playerInput.actions["Move"];
        m_shootVectorAction = m_playerInput.actions["Shoot Vector"];
        m_shootButtonAction = m_playerInput.actions["Shoot Button"];
        m_interactAction = m_playerInput.actions["Interact"];
        m_cancelAction = m_playerInput.actions["Cancel"];
    }

    void Update()
    {
        if (m_state != PlayerState.Default) return;

        //Move Character
        Vector2 velocity = m_moveAction.ReadValue<Vector2>() * m_speed;

        //Get Shoot Vector
        {
            m_shootVector = m_shootVectorAction.ReadValue<Vector2>();
            if (m_shootVector == Vector2.zero && m_shootButtonAction.IsPressed() && m_playerIndex <= 0)
            {
                m_shootVector = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) -
                    ((Vector2)transform.position + (Vector2.up * m_projectileYOffset));
            }
            if (m_shootVector != Vector2.zero)
            {
                m_shootVector.Normalize();
            }
        }

        //Set Velocity
        m_rigidbody.velocity = velocity;

        if (Keyboard.current.oKey.isPressed) Kill();
    }

    void LateUpdate()
    {
        if (m_state != PlayerState.Default) return;

        //Flip Character
        if (Mathf.Abs(m_rigidbody.velocity.x) > 0.01f) m_spriteRenderer.flipX = m_rigidbody.velocity.x <= 0.0f;
        m_animator.SetFloat("Speed", m_rigidbody.velocity.sqrMagnitude);

        //Shoot Mouse
        if (m_shootVector != Vector2.zero)
        {
            //Flip Character
            m_spriteRenderer.flipX = m_shootVector.x < 0.0f;
        }
        
        m_animator.SetBool("Attacking", m_shootVector.sqrMagnitude > 0.0f);
    }

    void OnCollisionEnter2D(Collision2D _col)
    {
        
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        
    }

    //Shoot a projectile
    void Shoot()
    {
        if (m_state != PlayerState.Default) return;

        Rigidbody2D projectile = Instantiate(m_projectile, transform.position + (Vector3.up * m_projectileYOffset), Quaternion.identity);
        projectile.velocity = m_shootVector * m_projectileSpeed;
        Destroy(projectile.gameObject, m_projectileLifetime);
    }

    //Interact with elements in the enviroment
    void Interact()
    {
        if (m_state != PlayerState.Default) return;
    }

    //Kill The player
    public void Kill()
    {
        //Trigger Death Animation
        m_animator.SetTrigger("Kill");

        //Disable Script
        enabled = false;

        //Update Gamemode
        GameMode.m_current.m_players.Remove(this);
        GameMode.m_current.OnPlayerKilled();
    }
}
