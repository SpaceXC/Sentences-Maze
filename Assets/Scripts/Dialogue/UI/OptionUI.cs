using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    public Button thisButton;
    private DialoguePiece currentPiece;
    private string nextPieceID;
    private bool IsCorrect;

    private void Awake()
    {
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        IsCorrect = option.isCorrect;
    }

    public void OnOptionClicked()
    {
        Debug.Log("你点击了一个选项！");
        Debug.Log(IsCorrect);
        FindObjectOfType<GameManager>().data.completion++;
        if (IsCorrect)
        {
            if (nextPieceID == "")
            {
                FindObjectOfType<GameManager>().currentRoom.DoorOpen();
                FindObjectOfType<GameManager>().NextRoom(true) ;
                FindObjectOfType<GameManager>().data.score++;
                StartCoroutine(TtsDemo.Instance.answerCorrrect());
            }
            else
            {
                DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
            }
        }
        else
        {
            
            FindObjectOfType<GameManager>().AnswerWrong();
            FindObjectOfType<GameManager>().NextRoom(false);
            StartCoroutine(TtsDemo.Instance.answerWrong());
        }
    }
}
