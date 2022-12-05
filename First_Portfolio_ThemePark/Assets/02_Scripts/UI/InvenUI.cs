using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InvenUI : MonoBehaviour
{
    public GameObject LeftInven;
    public GameObject RightInven;

    public TMP_Text mLeftInvenText;
    public TMP_Text mRightInvenText;

    bool mbHasLeftInven = false;
    bool mbHasRightInven = false;

    private void Update()
    {
        if(Input.GetKeyDown("j"))
        {
            UpdateInven(ref mLeftInvenText, LeftInven, ref mbHasLeftInven);
            UpdateInven(ref mRightInvenText, RightInven, ref mbHasRightInven);
        }
    }
    void UpdateInven(ref TMP_Text _invenText, GameObject _inven, ref bool _bHasInven)
    {
        if(_inven.GetComponentInChildren<Item>()==null)
        {
            _invenText.text = "Empty";
        }
        else
        {
            _invenText.text = _inven.GetComponentInChildren<Item>().name;
            UpdateInvenBool(ref _bHasInven);
        }
    }

    bool UpdateInvenBool(ref bool _bHasInven)
    {
        _bHasInven = !_bHasInven;
        return _bHasInven;
    }
}
