using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed = 5f;

    private PlayerActionInput playerMoveInput;
    private PlayerRotateInput playerRotateInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // ������Ʈ �۵� �� �ʱ�ȭ, ����� ������Ʈ ������ ��������
        playerMoveInput = GetComponent<PlayerActionInput>();
        playerRotateInput = GetComponent<PlayerRotateInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();

        // �ȱ� �ִϸ��̼� �۵�
        playerAnimator.SetFloat("Walk", playerMoveInput.move_x + playerMoveInput.move_z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move()
    {
        // �̵� ���� ���̽�ƽ �� ���� �� �̵� ����
        float x_speedvalue = playerMoveInput.move_x;
        float z_speedvalue = playerMoveInput.move_z;
        Vector3 move_speedvalue = new Vector3(x_speedvalue, 0f, z_speedvalue);

        playerRigidbody.MovePosition(playerRigidbody.position + move_speedvalue.normalized * Speed * Time.deltaTime);

    }

    private void Rotate()
    {
        // ȸ�� ���� ���̽�ƽ �� ���� �� ȸ�� ����
        float x_rotatevalue = playerRotateInput.rotate_x * Speed;
        float z_rotatevalue = playerRotateInput.rotate_z * Speed;

        Vector3 turn_rotatevalue = new Vector3(x_rotatevalue, 0f, z_rotatevalue);

        transform.LookAt(transform.position + turn_rotatevalue);

    }

}
