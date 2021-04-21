using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerStats { Stop,Moving}
public class PlayerController : MonoBehaviour
{
    public Transform lookAtPoint;
    public PlayerStats playerStats;

    /*private void Update()
    {
        switch (playerStats)
        {
            case PlayerStats.Moving:
                transform.LookAt(lookAtPoint);
                break;
            case PlayerStats.Stop:
                transform.LookAt(transform.forward);
                break;
        }
    }*/
}
