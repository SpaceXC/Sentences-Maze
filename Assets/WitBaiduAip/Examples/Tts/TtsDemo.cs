using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Lean.Gui;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;

public class TtsDemo : Singleton<TtsDemo>
{
    public string APIKey = "";
    public string SecretKey = "";

    private Tts _asr;
    private AudioSource _audioSource;
    private bool _startPlaying;

    [Header("声音效果")] 
    public AudioClip wrong;
    public AudioClip win;
    public AudioClip fail;
    public AudioClip correct;
    
    void Start()
    {
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        _audioSource = gameObject.AddComponent<AudioSource>();

    }
    public void TTS(string str)
    {
        Debug.Log(_asr+"   "+str);
        Debug.Log("合成中...");

        StartCoroutine(_asr.Synthesis(str, s =>
        {
            if (s.Success)
            {
                Debug.Log("合成成功，正在播放");
                _audioSource.clip = s.clip;
                _audioSource.Play();

                _startPlaying = true;
            }
            else
            {
                Debug.Log(s.err_msg);
            }
        }));
    }
    #region PlayeSound

    public IEnumerator answerCorrrect()
        {
           _audioSource.PlayOneShot(correct);
           yield break;
        }
    public IEnumerator answerWrong()
    {
        _audioSource.PlayOneShot(wrong);
        yield break;
    }
    public IEnumerator gameWin()
    {
        _audioSource.PlayOneShot(win);
        yield break;
    }
    public IEnumerator gameFailed() 
    {
        _audioSource.PlayOneShot(fail);
        yield break;
    }

    #endregion
    
}