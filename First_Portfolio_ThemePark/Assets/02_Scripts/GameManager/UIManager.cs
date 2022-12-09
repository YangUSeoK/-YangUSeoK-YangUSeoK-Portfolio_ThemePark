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

    // 게임오버 시 호출 됨
   public void IsGameOver()
    {
        // 게임오버 UI 띄워주기
        //2022 12 01 김준우
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
        // Inventory UI 띄워주기
    }
    public void CloseInventory()
    {
        // Inventory UI 끄기
    }

}
