using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManagement : MonoBehaviour
{
    [SerializeField] private int m_spawnCount;
    [SerializeField] private float m_maxSpawnTimer;
    [SerializeField] private float m_currentSpawnTimer;
    [SerializeField] private List<GameObject> m_enemySpawnable;
    [SerializeField] private List<Spawner> m_allSpawners;
    
    private void Start()
    {
        m_currentSpawnTimer = m_maxSpawnTimer;
        StartCoroutine(AllSpawnFound());
    }

    private void Update()
    {
        m_currentSpawnTimer -= Time.deltaTime;
        if (m_currentSpawnTimer <= 0)
        {
            if (SpawnEnemy())
                m_currentSpawnTimer = m_maxSpawnTimer;
        }
    }

    private bool SpawnEnemy()
    {
        Vector3 newPos = CalculateSpawn();
        if (newPos == Vector3.one * -1)
            return false;
        Instantiate(m_enemySpawnable[0], newPos, Quaternion.identity);
        return true;
    }

    private Vector3 CalculateSpawn()
    {
        Vector3 spawnPoint = Vector3.one * -1;
        List<Vector3> potentialPoints = new List<Vector3>();
        for (int i = 0; i < m_allSpawners.Count; i++)
        {
            if (!m_allSpawners[i].m_renderer.isVisible && m_allSpawners[i].gameObject.activeInHierarchy)
            {
                potentialPoints.Add(m_allSpawners[i].transform.position);
            }
        }

        if (potentialPoints.Count < 1)
            return spawnPoint;

        spawnPoint = potentialPoints[Random.Range(0, potentialPoints.Count)];
        return spawnPoint;
    }

    IEnumerator AllSpawnFound()
    {
        yield return new WaitForSeconds(0.1f);
        var allSpawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        for (int i = 0; i < allSpawners.Count; i++)
        {
            m_allSpawners.Add(allSpawners[i].GetComponent<Spawner>());
        }
    }
}
