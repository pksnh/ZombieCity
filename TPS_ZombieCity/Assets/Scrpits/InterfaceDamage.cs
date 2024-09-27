using UnityEngine;

public interface InterfaceDamage
{
    // �������� ���� �� �ִ� Ÿ�Ե��� InterfaceDamage�� ����ϰ� OnDamage �޼��带 �ݵ�� �����ؾ� �Ѵ�
    // OnDamage �޼���� �Է����� ������ ũ��(damage), ���� ����(hitPoint), ���� ǥ���� ����(hitNormal)�� �޴´�
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
