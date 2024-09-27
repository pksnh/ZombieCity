using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionInput : MonoBehaviour
{

    public VariableJoystick joystick;
    public float move_x { get; private set; }
    public float move_z { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 위한 조이스틱 정보 값 받기
        move_x = joystick.Horizontal;
        move_z = joystick.Vertical;
    }
}
