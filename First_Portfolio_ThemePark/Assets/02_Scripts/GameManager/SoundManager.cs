using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource bgSound;

    public AudioClip[] bglist;

    private void Awake()
    {
        if (instance == null)
        {
            bgSound = GetComponent<AudioSource>();
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

    public void SFXPlay(string sfxName, AudioClip clip, float pitch = 1f)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.pitch = pitch;
        audiosource.Play();
        Destroy(go, clip.length);
    }
    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.5f;
        bgSound.Play();
    }


    #region Enemy_State_BGM
    public void SetPatrolBGM()
    {
        // ���� ���
        Debug.Log("PatrolBGM");
    }

    public void SetTracePlayerBGM()
    {
        // if(TracePlayer ����� �ƴ϶��)
        // TracePlayer ���·� ��� �ٲ�
        bgSound.Stop();
        SFXPlay("TracePlayer", bglist[3]);
        Debug.Log("SetTracePlayerBGM");
    }

    public void FadeOutTracePlayerBGM()
    {
        // TracePlayer���� ������ ���� ���̵�ƿ�
        bgSound.Play();
        Debug.Log("ExitTracePlayerBGM");
    }

    // ���ӿ��� �� ȣ���
    public void IsGameOver()
    {
        bgSound.Stop();
        //bgm.Stop();
        SFXPlay("GameOver", bglist[2], 0.5f);
        

        // ���ӿ��� ��� ���
        Debug.Log("GameOver");
    }
    #endregion

    public void IsGameClear()
    {
        bgSound.Stop();
        SFXPlay("GameClear", bglist[4]);
        Debug.Log("GameClear!");
    }
}



























