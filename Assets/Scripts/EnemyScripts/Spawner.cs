using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Renderer m_renderer { get; private set; }

    private void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }
}
