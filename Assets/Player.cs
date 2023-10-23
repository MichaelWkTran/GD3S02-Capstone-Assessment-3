using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float m_speed;

    [Header("Projectile")]
    [SerializeField] Rigidbody2D m_projectile;
    [SerializeField] float m_projectileSpeed;
    [SerializeField] float m_projectileLifetime;
    [SerializeField] float m_projectileYOffset;

    [Header("Components")]
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Animator m_animator;
    [SerializeField] Rigidbody2D m_rigidbody;

    void Update()
    {
        //Move Character
        Vector2 velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * m_speed;

        //Set Velocity
        m_rigidbody.velocity = velocity;
    }

    void LateUpdate()
    {
        //Shoot
        bool isAttacking = Input.GetMouseButton(0);
        if (Input.GetMouseButton(0))
        {
            
        
        }
        m_animator.SetBool("Attacking", Input.GetMouseButton(0));

        //Flip Character
        if (Mathf.Abs(m_rigidbody.velocity.x) > 0.01f) m_spriteRenderer.flipX = m_rigidbody.velocity.x <= 0.0f;
        m_animator.SetFloat("Speed", m_rigidbody.velocity.sqrMagnitude);

        //Shoot
        if (Input.GetMouseButton(0))
        {
            //Get Mouse Position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePosition.z = 0.0f;

            //Flip Character
            m_spriteRenderer.flipX = (mousePosition.x - transform.position.x) < 0.0f;
        }
        m_animator.SetBool("Attacking", Input.GetMouseButton(0));
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePosition.z = 0.0f;

        Rigidbody2D projectile = Instantiate(m_projectile, transform.position + (Vector3.up * m_projectileYOffset), Quaternion.identity);
        projectile.velocity = (mousePosition - (transform.position + (Vector3.up * m_projectileYOffset))).normalized * m_projectileSpeed;
        Destroy(projectile.gameObject, m_projectileLifetime);
    }
}
