using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] float m_health;
    public virtual float m_Health
    {
        get { return m_health; }
        set
        {
            m_health = value;
            if (m_health > 0.0f) return;
            Kill();
        }
    }
    public const float cameraDeleteDistance = 0.5f;

    public void Start()
    {
        InvokeRepeating("DeleteEnemyOffScreen", 0.0f, 0.1f);
    }

    void DeleteEnemyOffScreen()
    {
        bool isOffScreen = true;

        //Check whether the enemy is on any player camera
        foreach(Player player in FindObjectsOfType<Player>(false))
        {
            if (!player.enabled) continue;

            Vector2 viewportPoint = player.m_Camera.WorldToViewportPoint(transform.position);
            Vector2 clampedViewportPoint = new Vector2(
                Mathf.Clamp(viewportPoint.x, -cameraDeleteDistance, 1.0f + cameraDeleteDistance),
                Mathf.Clamp(viewportPoint.y, -cameraDeleteDistance, 1.0f + cameraDeleteDistance));

            isOffScreen = viewportPoint != clampedViewportPoint;
            if (isOffScreen == false) break;
        }

        //Delete the enemy if not on camera
        if (isOffScreen) Destroy(gameObject);
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
    }
}
