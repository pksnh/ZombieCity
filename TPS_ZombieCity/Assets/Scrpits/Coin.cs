using UnityEngine;

// ���� ������ ������Ű�� ������
public class Coin : MonoBehaviour, InterfaceItem
{
    public int score = 200; // ������ ����

    public void Use(GameObject target)
    {
        // ���� �Ŵ����� ������ ���� �߰�
        GameManager.instance.AddScore(score);
        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}