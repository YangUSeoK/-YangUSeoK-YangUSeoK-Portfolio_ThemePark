using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.Interactions;

public class Flashlight : Item
{
    EItemType eItemtype = EItemType.Flashlight;
    [SerializeField] GameObject m_Light;
    private Material[] m_Lens;
    private Color m_LensColor = new Color(0.75f, 0.75f, 0.75f);
    private bool mbIsOn = false;

    void Start()
    {
        m_Light.SetActive(mbIsOn);
        m_Lens = GetComponentInChildren<Renderer>().materials;
    }

    void Update()
    {
        Action();
    }

    protected override void ActionInterface()
    {
        //if (mbIsHanded == false)
        //    return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            mbIsOn = !mbIsOn;

            m_ItemAudio[0].Play();
            if (mbIsOn)
            {
                m_Light.SetActive(true);
                m_Lens[0].SetColor("_EmissionColor", Color.white);
                m_Lens[1].SetColor("_EmissionColor", m_LensColor);
            }
            else
            {
                m_Light.SetActive(false);
                m_Lens[0].SetColor("_EmissionColor", Color.black);
                m_Lens[1].SetColor("_EmissionColor", Color.black);
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        m_ItemAudio[1].Play();
    }
}
