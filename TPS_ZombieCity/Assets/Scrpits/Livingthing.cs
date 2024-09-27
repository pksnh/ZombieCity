using System;
using UnityEngine;

public class Livingthing : MonoBehaviour, InterfaceDamage
{
    public float startHealth = 100f; // ���� ü��
    public float health { get; protected set; } // ���� ü��
    public bool dead { get; protected set; } // ��� ����
    public event Action ActionDeath; // ����� �ߵ��� �̺�Ʈ, Action Ÿ��: deligate Ÿ�� �� �Ű� ������ ����, deligate Ÿ��: ������ �Լ��� ����(�Լ� ������ ����)->������() ȣ��� ���� �� �Լ��� ��� ȣ�� ����, event Ű����: ȣ���� �ش� Ŭ���������� ��

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� ����
    protected virtual void OnEnable() // �������� ���� ������Ʈx->awake ���x
    {
        // ������� ���� ���·� ����
        dead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        health = startHealth;
    }

    // �������� �Դ� ���
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // ��������ŭ ü�� ����
        health -= damage;
        // Debug.Log(health);
        // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // ü���� ȸ���ϴ� ���
    public virtual void HealHealth(float itemHealth)
    {
        if (dead)
        {
            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }

        // ü�� �߰�
        health += itemHealth;
    }

    // ��� ó��
    public virtual void Die()
    {
        // onDeath �̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (ActionDeath != null)
        {
            ActionDeath();
        }

        // ��� ���¸� ������ ����
        dead = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
