using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mGameOver;
    public GameObject mGameClear;
    // ���ӿ��� �� ȣ�� ��
   public void IsGameOver()
    {
        // ���ӿ��� UI ����ֱ�
        //2022 12 01 ���ؿ�
        mGameOver.SetActive(true);
    }
    public void IsGameClear()
    {
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
