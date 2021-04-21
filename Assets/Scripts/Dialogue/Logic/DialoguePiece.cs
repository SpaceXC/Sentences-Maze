using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{
    public string ID;
    [TextArea]
    public string text;

    public List<DialogueOption> option = new List<DialogueOption>();
}
