using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject door;
    public DialogueData_SO data;
    public DialogueController npc;
    public Transform playerPoint;


    public void DoorOpen()
    {
        LeanTween.moveLocalY(door, 4.78f, 1f);
        Debug.Log("门开了！");
    }

    public void DoorClose()
    {
        LeanTween.moveLocalY(door, 2.2942f, 1.5f);
    } 
}
