using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mGameOver;
    public GameObject mGameClear;
    // 게임오버 시 호출 됨
   public void IsGameOver()
    {
        // 게임오버 UI 띄워주기
        //2022 12 01 김준우
        mGameOver.SetActive(true);
    }
    public void IsGameClear()
    {
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
