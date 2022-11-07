using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SightMan : Enemy
{
    public float LookLightRange
    {
        get
        {
            return m_LookLightRange;
        }
    }
    [SerializeField] private float m_LookLightRange;

    private void Start()
    {
        GetComponent<EnemyAI_SightMan>().SetLookLightRange(m_LookLightRange);
    }


    protected override void Action()
    {
        
    }
}
