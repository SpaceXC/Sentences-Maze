using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongWord : MonoBehaviour
{
    public void SetupWrongWord(string str)
    {
        Text selfText = gameObject.GetComponent<Text>();
        selfText.text = str;
    }
}
