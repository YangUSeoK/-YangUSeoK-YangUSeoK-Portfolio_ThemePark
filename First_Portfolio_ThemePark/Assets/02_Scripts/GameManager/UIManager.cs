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

    // °ÔÀÓ¿À¹ö ½Ã È£Ãâ µÊ
    public void IsGameOver()
    {
        // °ÔÀÓ¿À¹ö UI ¶ç¿öÁÖ±â
        //2022 12 01 ±èÁØ¿ì
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
        // Inventory UI ¶ç¿öÁÖ±â
    }
    public void CloseInventory()
    {
        // Inventory UI ²ô±â
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
