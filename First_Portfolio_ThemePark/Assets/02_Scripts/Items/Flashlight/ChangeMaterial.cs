using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_ModelRenderer = null;
    [SerializeField] private Material m_Glass = null;
    [SerializeField] private Material m_RimLight = null;
    [SerializeField] private Material m_Standard = null;
    [SerializeField] private Material[] m_Materials = null;

    private Flashlight m_FlashLight = null;


    private void Awake()
    {
        m_FlashLight = GetComponent<Flashlight>();
        m_Materials = m_ModelRenderer.sharedMaterials;
    }
    private void Update()
    {
        if (!m_FlashLight.GetIsHanded())
        {
            m_Materials[0] = m_Glass;
            m_Materials[1] = m_Glass;
            m_Materials[2] = m_RimLight;

            m_ModelRenderer.sharedMaterials = m_Materials;
        }
        else
        {
            m_Materials[0] = m_Glass;
            m_Materials[1] = m_Glass;
            m_Materials[2] = m_Standard;

            m_ModelRenderer.sharedMaterials = m_Materials;
        }
    }


}
