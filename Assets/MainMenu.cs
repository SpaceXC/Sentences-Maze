using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

public class MainMenu : Singleton<MainMenu>
{
    public Text inputName;
    public LeanButton login;
    public GameObject currentCamera;
    public GameObject MainCamera;
    public GameObject playerHealth;
    public GameObject gameManager;
    public GameObject teacherPanel;
    public GameObject loginPanel;
    public LeanButton quit;
    
    void Awake()
    {
        login.OnClick.AddListener(Login);
        quit.OnClick.AddListener(Quit);
    }

    public void Login()
    {
        if (inputName.text.Length < 4) return;
        
        if (inputName.text == "英语老师")
        {
            gameManager.SetActive(true); 
            teacherPanel.SetActive(true);
        }
        else
        {
            loginPanel.SetActive(false);
            currentCamera.SetActive(false);
            MainCamera.SetActive(true); 
            playerHealth.SetActive(true); 
            gameManager.SetActive(true); 
            FindObjectOfType<GameManager>().data.name = inputName.text;
            FindObjectOfType<GameManager>().data.studentName = inputName.text;
            StartCoroutine(FindObjectOfType<GameManager>().Timer());
        }
    }

    private void Quit()
    {
        Application.Quit();
    }
}
