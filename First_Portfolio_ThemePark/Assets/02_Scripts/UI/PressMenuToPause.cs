using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressMenuToPause : MonoBehaviour//22 11 29 김준우
{
    public OnClickButton UIButton;
    bool isGameStop = false;

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start)&&isGameStop == false)
        {
            //Debug.Log("정상작동함");
            PressStartToGoMenu();
        }
        else if(OVRInput.GetDown(OVRInput.Button.Start) && isGameStop == true)
        {
            OutPauseGame();
        }
    }
    void PauseAllGame()//게임 일시정지
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        isGameStop = true;
    }
    void OutPauseGame()//게임 일시정지 해제
    {
        isGameStop = false;
        Time.timeScale = 1.0f;
        UIButton.QuitMenu();
    }
    public void PressStartToGoMenu()//왼쪽 메뉴 눌러 스타트로 이동
    {
        PauseAllGame();
        UIButton.GoToMenu();
    }
}
