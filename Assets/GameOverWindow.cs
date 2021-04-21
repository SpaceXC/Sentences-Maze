using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverWindow : Singleton<GameOverWindow>
{
    public Text text;
    public LeanButton restart;
    public LeanButton quit;

    private void Awake()
    {
        restart.OnClick.AddListener(Restart);
        quit.OnClick.AddListener(Quit);
    }

    public void SetupWindow(string str)
    {
        text.text = str;
        LeanTween.moveLocalY(gameObject, 0, 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quiting...");
    }
}
