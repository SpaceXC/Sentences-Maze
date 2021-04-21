using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public StudentData templateData;
    public StudentData data;
    public int health;
    private int Index = -1;
    [HideInInspector]
    public int currentHealth = 3;
    public int deathCount = 0;

    public Text dialogueText;

    [Header("房间生成")]
    public List<GameObject> roomList = new List<GameObject>();
    public GameObject roomPrefab;
    public Transform point;
    public Room1 currentRoom;
    public int zOffset;

    public GameObject player;

    public List<DialogueData_SO> DialogueLists = new List<DialogueData_SO>();

    public DialogueData_SO currentDialogue;
    public GameOverWindow window;

    public int TotalTime;
    public Text timer;


    public Error errorPanel;    

    private void OnEnable()
    {
        data = Instantiate(templateData);
        HealthUI.Instance.UpdateHealthBarUI();
        point.position = new Vector3(0, 0, 0);
        GenerateRoom();
        Index++;
        currentRoom = roomList[Index].GetComponent<Room1>();
        LeanTween.moveLocal(player, currentRoom.playerPoint.position, 2f);
        window = FindObjectOfType<GameOverWindow>();
        if (data.studentName == "英语老师") return;
        currentRoom.npc.OpenDialogue();
    }
    
    private void Update()
    {
        Debug.Log(currentRoom.gameObject);
        if (TotalTime == 0)
        {
            StartCoroutine(GameOver());
        }
    }

    public void AnswerWrong()
    {
        deathCount++;
        currentHealth--;
        if (!HaveWrong(currentDialogue.name))
        {
            data.WrongOptions.Add(currentDialogue.name);
        }
        if (deathCount == 3)
        {
            HealthUI.Instance.UpdateHealthBarUI();
            Debug.Log("死亡！");
            window.SetupWindow("You failed!");
            StopCoroutine(Timer());
            StartCoroutine(TtsDemo.Instance.gameFailed());
            Upload2Server();
            
        }
        else
        {
            HealthUI.Instance.UpdateHealthBarUI();
        }
    }

    public void UpdateScore(StudentData data)
    {
        data.score += 1;
    }

    public void Quit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    public void NextRoom(bool correct)
    {
        if (Index < roomList.Count - 1)
        {
            if (correct)
            {
                Index++;
                currentRoom = roomList[Index].GetComponent<Room1>();
                /*playerController.lookAtPoint = currentRoom.playerPoint;
                playerController.playerStats = PlayerStats.Moving;*/
                LeanTween.moveLocal(player, currentRoom.playerPoint.position,2f);
                currentRoom.npc.OpenDialogue();
                /*playerController.playerStats = PlayerStats.Stop;*/
            }
            else
            {
                return;
            }
        }
        else
        {
            Debug.Log("你成功了！！！");
            StartCoroutine(TtsDemo.Instance.gameWin());
            window.SetupWindow("You win!");
            StopCoroutine(Timer());
            Upload2Server();
        }
    }

    public void GenerateRoom()
    {
        for (int i = 0; i < DialogueLists.Count; i++)
        {
            GameObject room = Instantiate(roomPrefab, point.position, Quaternion.identity);
            //room.GetComponent<Room>().data = DialogueLists[i];
            room.GetComponent<Room1>().npc.currentData= DialogueLists[i]; 
            roomList.Add(room);
            room.name = "Room no." + (i + 1).ToString();
            int suijishu = UnityEngine.Random.Range(1,100);
            if (suijishu % 2 == 1)
            {
                point.position += new Vector3(0, 0, -4.5f);
                print(point.position);
            }
            else
            {
                point.position += new Vector3(-4.5f, 0, 0);
                print(point.position);
            }
        }
    }

    public void Upload2Server()
    {
        string json = JsonUtility.ToJson(data);
        Upload.Instance.UploadFile(json,data.studentName);
    }
    public IEnumerator Timer()
    {
        while (TotalTime >= 0)
        {
            timer.GetComponent<Text>().text = TotalTime.ToString();
            yield return new WaitForSeconds(1);
            TotalTime--;
        }
    }

    IEnumerator GameOver()
    {
        Debug.Log("Time's Up!");
        yield return StartCoroutine(TtsDemo.Instance.gameFailed());
        window.SetupWindow("Time's Up!");
        Upload2Server();
    }

    public bool HaveWrong(string wrongIndex)
    {
        if (wrongIndex != null)
        {
            return data.WrongOptions.Any(i => i == wrongIndex);
        }
        else return false;
    }
}