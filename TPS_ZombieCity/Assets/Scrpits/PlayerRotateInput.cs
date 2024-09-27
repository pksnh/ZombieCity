using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateInput : MonoBehaviour
{
    public VariableJoystick joystick;
    public float rotate_x { get; private set; }
    public float rotate_z { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 회전 위한 조이스틱 정보 값 받기
        rotate_x = joystick.Horizontal;
        rotate_z = joystick.Vertical;
    }
}
