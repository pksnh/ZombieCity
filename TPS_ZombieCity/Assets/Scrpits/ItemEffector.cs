using UnityEngine;

public class ItemEffector : MonoBehaviour
{
    // 게임 오브젝트가 지속적으로 회전하게 하는 스크립트
    public float rotationSpeed = 60f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}