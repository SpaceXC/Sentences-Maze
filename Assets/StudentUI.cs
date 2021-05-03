using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class StudentUI : Singleton<StudentUI>
{
    [Header("Basic Elements")] 
    public Text studentName;
    public Text compleption;
    public Text score;
    public RectTransform wrongWordsTransform;
    public WrongWord wrongWord;

    public void SetupPanel(StudentData data)
    {
        studentName.text = data.studentName;
        compleption.text = data.completion + " / " + FindObjectOfType<GameManager>().DialogueLists.Count.ToString();
        score.text = data.score.ToString();

        foreach (Transform i in wrongWordsTransform)
        {
            Destroy(i.gameObject);
        }

        foreach (string word in data.WrongOptions)
        {
            WrongWord newWord=Instantiate(wrongWord, wrongWordsTransform);
            newWord.SetupWrongWord(word);
        }
    }

    public void Clear()
    {
        studentName.text = "";
        compleption.text = "";
        score.text = "";
        for (int i = 0; i < wrongWordsTransform.childCount; i++)
        {
            Destroy(wrongWordsTransform.transform.GetChild(i).gameObject);
        }
    }
}
