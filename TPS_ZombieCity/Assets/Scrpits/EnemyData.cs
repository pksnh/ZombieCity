using UnityEngine;

// �� ������ ����� �¾� ������
[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    // float�� �νĵ�, int�� �� �ν� �� ����?

    public float health = 100f; // ü��
    public float damage = 20f; // ���ݷ�
    public float speed = 2f; // �̵� �ӵ�
    public float killscore = 100; // ��� �� ����
    public Color Outlinecolor; // �׵θ� ����
    public float Outlinethick; // �׵θ� �α�
}