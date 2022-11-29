using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] m_SpawnPoints = null;
    public SpawnPoint[] SpawnPoints
    {
        get { return m_SpawnPoints; }
    }

    private void Awake()
    {
        m_SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }
}
