using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Error : Singleton<Error>
{
    public Text text;
    public LeanButton ok;
    public LeanButton quit;
    public GameObject window;

    void Awake()
    {
        ok.OnClick.AddListener(OK);
        quit.OnClick.AddListener(Quit);
    }
    
    public void SetupErrorUI(string str)
    {
        window.SetActive(true);
        text.text = str;
    }

    public void OK()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
