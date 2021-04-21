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

    int wordIndex = 0;

    public StudentData templateData;
    public StudentData data;

    public Text scoreText;

    public GameObject loginPanel;
    public GameObject wordPanel;
    public LeanButton login;
    public Text name;
    public Text nameText;

    //public PlayerController player;

    public PlayableDirector Director;

    public LeanButton quitBtn;

    public GameObject teacherCanvas;
    public int TotalTime = 150;
    public Text timer;
    public PlayableDirector gameoverTimeline;

    public LeanButton notiQuitBtn;
    public LeanButton restart;

    public Button clear;

    [Header("声音效果")] 
    public AudioClip wrong;
    public AudioClip win;
    public AudioClip fail;
    public AudioClip correct;
    
    
    [System.Serializable]
    public class WordList
    {
        public string word;
    }
    
    //public List<WordController.WordList> WordLists=new List<WordController.WordList>();
    
    void Start()
    {
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

        _audioSource = gameObject.AddComponent<AudioSource>();

    }

    void Update()
    {
        if (_startPlaying)
        {
            if (!_audioSource.isPlaying)
            {
                _startPlaying = false;
            }
        }
        if (TotalTime == 0)
        {
            Debug.Log("game over");
            GameOver();
        }
        print(_asr);
    }

    /*public void UpdateWord()
    {
        
        string currentWord = WordLists[wordIndex].word;

        if (wordIndex < WordLists.Count-1)
        {
            //TODO:加一分
            
            if (Input.text == currentWord)
            {
                ClearInputField();
                UpdateScore(data);
                scoreText.text = data.score.ToString();
                wordIndex += 1;
                currentWord = WordLists[wordIndex].word;
                player.PlayerMove();
                TTS("Wonderful!The next word is : "+currentWord);
            }
            else
            {
                data.WrongWords.Add(currentWord);
                wordIndex += 1;
                currentWord = WordLists[wordIndex].word;
                player.PlayerMove();
                TTS("Oops!Your answer was wrong!The next word is : "+currentWord);
            }
        }
        else
        {
            if (Input.text == currentWord)
            {
                UpdateScore(data);
                scoreText.text = data.score.ToString();
                player.PlayerMove();
                StopCoroutine(Timer());
                data.completion = wordIndex;
                string json = JsonUtility.ToJson(data);
                Upload.Instance.UploadFile(json,data.studentName);
                TTS("congratulations!");
                quitBtn.gameObject.SetActive(true);
            }
            else
            {
                data.WrongWords.Add(currentWord);
                player.PlayerMove();
                StopCoroutine(Timer());
                data.completion = wordIndex;
                string json = JsonUtility.ToJson(data);
                Upload.Instance.UploadFile(json, data.studentName);
                TTS("congratulations!");
                quitBtn.gameObject.SetActive(true);
            }

        }
        
    }*/

    public void UpdateScore(StudentData data)
    {
        data.score += 1;
    }

    
    public void Login()
    {
        if (name.text == "") return;
        if (name.text == "英语老师")
        {
            loginPanel.gameObject.SetActive(false);
            teacherCanvas.SetActive(true);
        }
        else
        {
            data.studentName = name.text;
            Director.Play();
            nameText.text = data.studentName;
        }
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
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
    
    IEnumerator Timer()
    {
        while (TotalTime >= 0)
        {
            timer.GetComponent<Text>().text = TotalTime.ToString();
            yield return new WaitForSeconds(1);
            TotalTime--;
        }
    }

    public void GameOver()
    {
        gameoverTimeline.Play();
        data.completion = wordIndex;
        string json = JsonUtility.ToJson(data);
        Upload.Instance.UploadFile(json,data.studentName);
        quitBtn.gameObject.SetActive(true);
    }

    public void GameStart(PlayableDirector meiyouyong)
    {
        StartCoroutine(Timer());
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Clear()
    {
        foreach (var file in Upload.Instance.studentManager.StudentsList)
        {
            if(DeleteFile(file.studentName + ".json"))
                print("True");
            else
            {
                print("false");
            }
            DeleteFile(file.studentName + ".json");
            Debug.Log("删除:"+file.studentName + ".json");
        }
        Upload.Instance.studentManager.RefreshUI();
    }
    
    public static bool DeleteFile(string fileName)
    {
        try
        {
            string url = "ftp://"+Upload.Instance.ServerIP+"/data/" + fileName;
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFtp.UseBinary = true;
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
            reqFtp.Credentials = new NetworkCredential("Xiaochang", "MangoLiu2190");
            FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
            response.Close();
            return true;
        }
        catch (Exception ex)
        {
            //errorinfo = string.Format("因{0},无法下载", ex.Message);
            return false;
        }
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