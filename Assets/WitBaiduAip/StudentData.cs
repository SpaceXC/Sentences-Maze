using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Student Data")]
public class StudentData : ScriptableObject
{
    public string studentName;
    public int score=-1;
    public int completion;
    
    public List<string> WrongOptions=new List<string>();
}
