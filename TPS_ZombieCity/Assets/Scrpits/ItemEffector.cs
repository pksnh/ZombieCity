using UnityEngine;

public class ItemEffector : MonoBehaviour
{
    // ���� ������Ʈ�� ���������� ȸ���ϰ� �ϴ� ��ũ��Ʈ
    public float rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}