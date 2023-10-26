using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Player : MonoBehaviour
{
    [SerializeField] float m_speed;

    [Header("Projectile")]
    [SerializeField] Rigidbody2D m_projectile;
    [SerializeField] float m_projectileSpeed;
    [SerializeField] float m_projectileLifetime;
    [SerializeField] float m_projectileYOffset;

    [Header("Game Mode")]
    public uint m_playerIndex;

    [Header("Inputs")]
    [SerializeField] PlayerInput m_playerInput;
    InputAction m_moveAction;
    InputAction m_shootVectorAction;
    InputAction m_shootButtonAction;
    InputAction m_interactAction;
    InputAction m_cancelAction;
    Vector2 m_shootVector;

    [Header("Components")]
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

        //Swap to Keyboard and Mouse
        if (m_playerIndex == 0U)
        {
            m_playerInput.SwitchCurrentControlScheme
            (
                "Player 1 Keyboard and Mouse",
                InputSystem.devices.Where (device => device.layout.Contains("Mouse") || device.layout.Contains("Keyboard")).ToArray()
            );
        }
    }

    void Update()
    {
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
    }

    void LateUpdate()
    {
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

    void Shoot()
    {
        Rigidbody2D projectile = Instantiate(m_projectile, transform.position + (Vector3.up * m_projectileYOffset), Quaternion.identity);
        projectile.velocity = m_shootVector * m_projectileSpeed;
        Destroy(projectile.gameObject, m_projectileLifetime);
    }

    public void Interact()
    {
        Debug.Log("Interact");
    }
}
