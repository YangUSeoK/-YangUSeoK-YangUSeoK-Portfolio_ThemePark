using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickButton : MonoBehaviour
{
    public GameObject mMenu;
    public GameObject mOptionBtn;
    public GameObject mGameOver;
    public GameObject mGameClear;

    [Header("튜토리얼(물음표) UI")]
    public GameObject[] mTutorialUI;
    private int mTutorialCnt = 0;

    public void StartGame()
    {
        mMenu.SetActive(false); 
        SceneManager.LoadSceneAsync("Silent_Escape");
    }
    public void GoToOptions()
    {
        mOptionBtn.SetActive(true);
        mMenu.SetActive(false);
    }
    public void GoToMenu()
    {
        mMenu.SetActive(true);
        mOptionBtn.SetActive(false);
    }
    public void GetOutGameScene()
    {
        mMenu.SetActive(false);
        mGameOver.SetActive(false);
        mGameClear.SetActive(false);
        SceneManager.LoadSceneAsync("MainOpen");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void QuitMenu()
    {
        mMenu.SetActive(false);
    }
    public void GoTutorial()
    {
        mMenu.SetActive(false);
        mTutorialUI[0].SetActive(true);
    }
    public void TutorialGotoMenu()
    {
        for (int i = 0; i < mTutorialUI.Length; i++)
        {
            mTutorialUI[i].SetActive(false);
        }
        mMenu.SetActive(true);
    }
    public void TutorialGoRight()
    {
        if(mTutorialCnt<mTutorialUI.Length)
        {
            mTutorialUI[mTutorialCnt].SetActive(false);
            ++mTutorialCnt;
            mTutorialUI[mTutorialCnt].SetActive(true);
        }
        else if(mTutorialCnt>=mTutorialUI.Length)
        {
            mTutorialCnt = 0;
        }
    }
    public void TutorialGoLeft()
    {
        if (mTutorialCnt < mTutorialUI.Length)
        {
            mTutorialUI[mTutorialCnt].SetActive(false);
            --mTutorialCnt;
            mTutorialUI[mTutorialCnt].SetActive(true);
        }
        else if (mTutorialCnt <= mTutorialUI.Length)
        {
            mTutorialCnt = mTutorialUI.Length;
        }
    }
}
