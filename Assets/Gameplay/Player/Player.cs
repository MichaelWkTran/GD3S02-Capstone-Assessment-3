using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerState m_state;
    float m_maxHealth = 100.0f;
    [SerializeField] float m_health = 100.0f;
    public float m_Health
    {
        get {  return m_health; }
        set
        {
            m_health = Mathf.Clamp(value, 0.0f, m_maxHealth);
            if (m_health <= 0.0f) Kill();

            //Update Health UI
            LeanTween.value(gameObject, m_healthBar.value, m_health, 0.2f).setEaseOutCirc()
            .setOnUpdate((float _newUIValue) =>
            {
                m_healthBar.value = _newUIValue;
                m_healthBar.fillRect.GetComponent<Image>().color = m_healthBarGradient.Evaluate(m_healthBar.value / m_maxHealth);
            });
        }
    }
    bool m_isInDamageInvincibility = false;
    [SerializeField] float m_damageInvincibilityTime;

    [Header("Inputs")]
    Vector2 m_shootVector;

    [Header("Components")]
    [SerializeField] Camera m_camera; public Camera m_Camera { get { return m_camera; } }
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Animator m_animator;
    [SerializeField] Rigidbody2D m_rigidbody;

    [Header("UI")]
    [SerializeField] Slider m_healthBar;
    [SerializeField] Gradient m_healthBarGradient;

    void Awake()
    {
        //Set UI
        m_healthBar.minValue = 0.0f;
        m_healthBar.maxValue = m_maxHealth;
        m_healthBar.value = m_health;
    }

    void Start()
    {
        m_spriteRenderer.material = GameMode.m_current.m_playerMaterials[m_playerIndex];
    }

    void Update()
    {
        if (m_state != PlayerState.Default) return;

        //Get Inputs
        int inputIndex = GameManager.m_Current.m_playerInputIndex[m_playerIndex];
        #region Vector2 moveInput = new Vector2(Input.GetAxis("leftstick horizontal"), Input.GetAxis("rightstick vertical"));
        Vector2 moveInput = Vector2.zero;
        if (inputIndex < 4)
        {
            moveInput = new Vector2
            (
                Input.GetAxisRaw("leftstick" + (inputIndex+1) + "horizontal"),
                Input.GetAxisRaw("leftstick" + (inputIndex+1) + "vertical")
            );
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) moveInput += Vector2.right;
            if (Input.GetKey(KeyCode.LeftArrow)  || Input.GetKey(KeyCode.A)) moveInput -= Vector2.right;
            if (Input.GetKey(KeyCode.UpArrow)    || Input.GetKey(KeyCode.W)) moveInput += Vector2.up;
            if (Input.GetKey(KeyCode.DownArrow)  || Input.GetKey(KeyCode.S)) moveInput -= Vector2.up;
        }
        moveInput.Normalize();

        #endregion
        #region m_shootVector = new Vector2(Input.GetAxis("rightstick horizontal"), Input.GetAxis("rightstick vertical"));
        m_shootVector = Vector2.zero;
        if (inputIndex < 4)
        {
            m_shootVector = new Vector2
            (
                Input.GetAxisRaw("rightstick" + (inputIndex + 1) + "horizontal"),
                Input.GetAxisRaw("rightstick" + (inputIndex + 1) + "vertical")
            );
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                m_shootVector = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                    ((Vector2)transform.position + (Vector2.up * m_projectileYOffset));
            }
        }
        m_shootVector.Normalize();

        #endregion

        //Move Character
        Vector2 velocity = moveInput * m_speed;

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

    //Damage the player
    public void OnDamage(float _damage)
    {
        if (!enabled) return;

        //Don't damage the player when in the damage invincibility state
        if (m_isInDamageInvincibility) return;

        //Sprite Flash
        LeanTween.value(gameObject, 0.0f, 1.0f, 0.5f).setEasePunch()
            .setOnUpdate((float _flashAlpha) => { m_spriteRenderer.material.SetFloat("_FlashAlpha", _flashAlpha); });

        //Damage Player
        m_Health -= _damage;

        //Vibrate Controller
        //IEnumerator MotorVibration()
        //{
        //    Gamepad gamepad = m_playerInput.GetDevice<Gamepad>();
        //    if (gamepad == null) yield break;
        //
        //    gamepad.SetMotorSpeeds(20.0f, 20.0f);
        //    yield return new WaitForSeconds(0.15f);
        //    gamepad.SetMotorSpeeds(0.0f, 0.0f);
        //}
        //StartCoroutine(MotorVibration());
        
        //Set Invincible for a Period of Time
        m_isInDamageInvincibility = true;

        IEnumerator WaitAndPrint()
        {
            yield return new WaitForSeconds(m_damageInvincibilityTime);
            m_isInDamageInvincibility = false;
        }
        StartCoroutine(WaitAndPrint());
    }

    //Shoot a projectile
    void Shoot()
    {
        if (m_state != PlayerState.Default) return;

        Rigidbody2D projectile = Instantiate(m_projectile, transform.position + (Vector3.up * m_projectileYOffset), Quaternion.identity);
        projectile.velocity = m_shootVector * m_projectileSpeed;
        projectile.GetComponent<SpriteRenderer>().color = GameMode.m_current.m_playerColours[m_playerIndex];
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
        if (!enabled) return;

        //Trigger Death Animation
        m_animator.SetTrigger("Kill");

        //Disable Script
        enabled = false;

        //Update Gamemode
        GameMode.m_current.m_players.Remove(this);
        GameMode.m_current.OnPlayerKilled(this);
    }
}
