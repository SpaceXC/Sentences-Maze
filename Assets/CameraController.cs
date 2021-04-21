using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FixedJoystick joystick;
    public float sensivity;

    private void Update()
    {
        float horizontalJoystick = joystick.Horizontal;

        transform.Rotate(0, horizontalJoystick * sensivity, 0);
    }
}
