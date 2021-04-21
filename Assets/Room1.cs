using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1 : MonoBehaviour
{
    public GameObject door;
    public GameObject door1;
    public DialogueData_SO data;
    public DialogueController npc;
    public Transform playerPoint;


    public void DoorOpen()
    {
        LeanTween.moveLocalY(door, 4.78f, 1f);
        LeanTween.moveLocalY(door1, 4.78f, 1f);
        Debug.Log("门开了！");
    }
}
