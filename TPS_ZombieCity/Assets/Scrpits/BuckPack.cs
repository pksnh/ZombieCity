using UnityEngine;

// �Ѿ��� �����ϴ� ������
public class BuckPack : MonoBehaviour, InterfaceItem
{
    public int ammo_buck = 10; // ������ �Ѿ� ��

    public void Use(GameObject target)
    {
        // ���� ���� ���� ������Ʈ�κ��� PlayerShooter ������Ʈ�� �������� �õ�
        Shooter playerShooter = target.GetComponent<Shooter>();

        // PlayerShooter ������Ʈ�� ������, �� ������Ʈ�� �����ϸ�
        if (playerShooter != null && playerShooter.gun != null)
        {
            playerShooter.gun.ammoRemain += ammo_buck;
        }

        // ���Ǿ����Ƿ�, �ڽ��� �ı�
        Destroy(gameObject);
    }
}