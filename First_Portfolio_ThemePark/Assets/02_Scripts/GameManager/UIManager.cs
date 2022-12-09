using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject mGameOver;
    public GameObject mGameClear;
    [SerializeField] private TMP_Text mGameOverText;
    [SerializeField] private Image mGameOverImage;

    // ���ӿ��� �� ȣ�� ��
   public void IsGameOver()
    {
        // ���ӿ��� UI ����ֱ�
        //2022 12 01 ���ؿ�
        Debug.Log("IsGameOver");
        mGameOver.SetActive(true);
        mGameOverImage.CrossFadeAlpha(0f, 0f, true);
        mGameOverText.CrossFadeAlpha(0f, 0f, true);
        mGameOverImage.CrossFadeAlpha(1f, 2f, true);
        mGameOverText.CrossFadeAlpha(1f, 2f, true);
    }
    public void IsGameClear()
    {
        Debug.Log("IsGameClear");
        mGameClear.SetActive(true);
    }

    public void OpenInventory()
    {
        // Inventory UI ����ֱ�
    }
    public void CloseInventory()
    {
        // Inventory UI ����
    }

}
