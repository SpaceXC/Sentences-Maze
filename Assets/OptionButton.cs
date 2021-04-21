using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    public bool IsCorrect;
    public Text text;

    private void Awake()
    {
        gameObject.GetComponent<LeanButton>().OnClick.AddListener(CheckOption);
    }

    public void SetupButton(string str, bool isCorrect)
    {
        text.text = str;
        IsCorrect = isCorrect;
    }

    public void CheckOption()
    {
        if (IsCorrect)
        {
            FindObjectOfType<GameManager>().currentRoom.DoorOpen();
        }
        else
        {
            FindObjectOfType<GameManager>().AnswerWrong();
        }
    }
}
