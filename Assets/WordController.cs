using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WordController : MonoBehaviour
{
    [System.Serializable]
    public class WordList
    {
        public string word;
    }
    
    public List<WordList> WordLists=new List<WordList>();
    
    
}
