using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using  UnityEngine.UI;

public class StudentNameButton : MonoBehaviour
{
    public StudentData currentData;
    public Text text;

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(SetupStudentInfoPanel);
    }

    public void SetupStudentNameButton(StudentData data)
    {
        currentData = data;
        text.text = currentData.studentName;
    }

    public void SetupStudentInfoPanel()
    {
        StudentUI.Instance.SetupPanel(currentData);
    }
}
