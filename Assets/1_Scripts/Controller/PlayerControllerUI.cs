using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    private JoyStickController joystick;

    private void Awake()
    {
        joystick = GameObject.FindObjectOfType<JoyStickController>();
    }
    // Start is called before the first frame update\
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            MoveControl();
    }

    private void MoveControl()
    {
        Vector3 upMovement = Vector3.up * speed * Time.deltaTime * joystick.Vertical;
        Vector3 rightMovement = Vector3.right * speed * Time.deltaTime * joystick.Horizontal;
        transform.position += upMovement;
        transform.position += rightMovement;
    }
}
