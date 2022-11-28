using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bgSound;
    public AudioClip[] bglist;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
        }

    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();
        Destroy(go, clip.length);
    }
    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }


    #region Enemy_State_BGM
    public void SetPatrolBGM()
    {
        // 평상시 브금
        Debug.Log("PatrolBGM");
    }

    public void SetTracePlayerBGM()
    {
        // if(TracePlayer 브금이 아니라면)
        // TracePlayer 상태로 브금 바뀜
        Debug.Log("SetTracePlayerBGM");
    }

    public void FadeOutTracePlayerBGM()
    {
        // TracePlayer에서 나갈때 볼륨 페이드아웃
        Debug.Log("ExitTracePlayerBGM");
    }

    // 게임오버 시 호출됨
    public void IsGameOver()
    {
        //bgm.Stop();

        // 게임오버 브금 출력
        Debug.Log("GameOver");
    }
    #endregion
}
























