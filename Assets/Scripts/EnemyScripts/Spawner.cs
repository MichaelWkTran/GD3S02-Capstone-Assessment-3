using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Renderer m_renderer { get; private set; }
    public bool insideMap = true;
    
    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("OutsideSpawn"))
        {
            insideMap = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("OutsideSpawn"))
        {
            insideMap = true;
        }
    }
}
