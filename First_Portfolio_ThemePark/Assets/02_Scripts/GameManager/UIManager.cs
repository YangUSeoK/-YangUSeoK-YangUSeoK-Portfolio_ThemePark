using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mGameOver;
    public GameObject mGameClear;
    public Image mImage = null;

    // ���ӿ��� �� ȣ�� ��
    public void IsGameOver()
    {
        // ���ӿ��� UI ����ֱ�
        //2022 12 01 ���ؿ�
        Debug.Log("IsGameOver");
        mGameOver.SetActive(true);
        StartCoroutine(FadeOutCoroutine());

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

    private IEnumerator FadeOutCoroutine()
    {
        Color color = mImage.color;

        while (color.a >= 2f)
        {
            color.a += 0.01f;
            mImage.color = color;
            yield return null;
        }
    }

}
