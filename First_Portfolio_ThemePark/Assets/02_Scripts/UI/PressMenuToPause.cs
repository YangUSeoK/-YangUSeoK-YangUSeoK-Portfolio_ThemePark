using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressMenuToPause : MonoBehaviour//22 11 29 ���ؿ�
{
    public OnClickButton UIButton;
    bool isGameStop = false;

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Start)&&isGameStop == false)
        {
            //Debug.Log("�����۵���");
            PressStartToGoMenu();
        }
        else if(OVRInput.GetDown(OVRInput.Button.Start) && isGameStop == true)
        {
            OutPauseGame();
        }
    }
    void PauseAllGame()//���� �Ͻ�����
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        isGameStop = true;
    }
    void OutPauseGame()//���� �Ͻ����� ����
    {
        isGameStop = false;
        Time.timeScale = 1.0f;
        UIButton.QuitMenu();
    }
    public void PressStartToGoMenu()//���� �޴� ���� ��ŸƮ�� �̵�
    {
        PauseAllGame();
        UIButton.GoToMenu();
    }
}
