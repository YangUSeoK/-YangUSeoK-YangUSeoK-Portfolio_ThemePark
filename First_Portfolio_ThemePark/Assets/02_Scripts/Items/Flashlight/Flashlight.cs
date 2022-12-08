using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.XR;

public class Flashlight : Item, IInteraction
{
    EItemType eItemtype = EItemType.Flashlight;
    [SerializeField] GameObject m_Light;
    [SerializeField] private Material[] m_Lens;
    private Color m_LensColor = new Color(0.75f, 0.75f, 0.75f);
    private bool mbIsOn = false;

    void Start()
    {
        m_Light.SetActive(mbIsOn);
        m_Lens = GetComponentInChildren<Renderer>().materials;
    }

    public override void Action()
    {
        //targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        if ((/*secondaryButtonValue||*/Input.GetKeyDown(KeyCode.X)) && mbIsHanded == true)
        {
            
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        m_ItemAudio[1].PlayOneShot(m_ItemAudio[1].clip);
        Debug.Log(m_ItemAudio[1].clip.loadState);
    }

    public void Interaction(bool _isTurnOn)
    {
        //mbIsOn = !mbIsOn;

        m_ItemAudio[0].PlayOneShot(m_ItemAudio[0].clip);
        if (_isTurnOn)
        {
            m_Rigid.isKinematic = true;
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

    public bool GetIsHanded()
    {
        return mbIsHanded;
    }
}
