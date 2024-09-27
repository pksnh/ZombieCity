using UnityEngine;

// ü���� ȸ���ϴ� ������
public class HealthPack : MonoBehaviour, InterfaceItem
{
    public float health = 50; // ü���� ȸ���� ��ġ

    public void Use(GameObject target)
    {
        // ���޹��� ���� ������Ʈ�κ��� livingThing ������Ʈ �������� �õ�
        Livingthing life = target.GetComponent<Livingthing>();

        // Livingthing ������Ʈ�� �ִٸ�
        if (life != null)
        {
            // ü�� ȸ�� ����
            life.HealHealth(health);
        }

        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}