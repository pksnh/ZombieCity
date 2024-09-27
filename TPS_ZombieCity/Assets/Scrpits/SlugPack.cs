using UnityEngine;

// �Ѿ��� �����ϴ� ������
public class SlugPack : MonoBehaviour, InterfaceItem
{
    public int ammo_slug = 4; // ������ �Ѿ� ��

    public void Use(GameObject target)
    {
        // ���� ���� ���� ������Ʈ�κ��� PlayerShooter ������Ʈ�� �������� �õ�
        Shooter playerShooter = target.GetComponent<Shooter>();

        // PlayerShooter ������Ʈ�� ������, �� ������Ʈ�� �����ϸ�
        if (playerShooter != null && playerShooter.gun1 != null)
        {
            playerShooter.gun1.ammoRemain += ammo_slug;
        }

        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}