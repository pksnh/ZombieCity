using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� �ý��� ���� �ڵ� ��������

// ���� AI ����
public class Enemy : Livingthing
{
    public LayerMask whatIsTarget; // ���� ��� ���̾�, ���̾�: ������ �׷���, ����ũ: ���������� �ɷ����� ����

    private Livingthing livingThing; // ���� ���, Livingthing ��� �޴� ��� ����, �÷��̾� ����+���� �����̵�
    private NavMeshAgent navMeshAgent; // ��� ��� AI ������Ʈ, UnityEngine.AI �ʿ�, zombie �̵���Ŵ

    public ParticleSystem hitEffect; // �ǰ� �� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ��� �� ����� �Ҹ�
    public AudioClip hitSound; // �ǰ� �� ����� �Ҹ�
    // public EnemyData enemyData; // �� ��ũ���ͺ� ������


    private Animator enemyAnimator; // �ִϸ����� ������Ʈ
    private AudioSource enemyAudioPlayer; // ����� �ҽ� ������Ʈ
    // private Renderer enemyRenderer; // ������ ������Ʈ
    private Outline outLine;

    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime; // ������ ���� ����

    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool tryTargeting // set ����->=���� �� ���� �Ұ�
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (livingThing != null && !livingThing.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private void Awake()
    {
        // �ʱ�ȭ
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        // enemyRenderer = GetComponentInChildren<Renderer>();
        outLine = GetComponentInChildren<Outline>();
    }

    // ���� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(EnemyData enemyData)
    {
        
        startHealth = enemyData.health;
        health = enemyData.health;
        damage = enemyData.damage;
        outLine.OutlineColor = enemyData.Outlinecolor;
        outLine.OutlineWidth = enemyData.Outlinethick;
        outLine.enabled = true;
        navMeshAgent.speed = enemyData.speed;
    }

    private void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath()); // StartCoroutine("UpdatePath");�� ����, ���� �ڷ�ƾ �Լ��� start()�� ������
    }

    private void Update()
    {
        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼� ���
        enemyAnimator.SetBool("Targeting", tryTargeting);
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        // ��� �ִ� ���� ���� ����, ������ ����
        while (!dead)
        {
            if (tryTargeting)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(livingThing.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 200f, whatIsTarget); // ���� ��ġ���� �����ؼ� ������ 500m �� �ȿ��� whatIsTarget ����, overlapSphere ���Ŀ��� �� ��

                for (int i = 0; i < colliders.Length; i++)
                {
                    Livingthing e = colliders[i].GetComponent<Livingthing>();
                    if (e != null && !e.dead)
                    {
                        livingThing = e; // livingThing�� ������ ��� ����
                        break;
                    }
                }
            }
            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �������� �Ծ��� �� ������ ó��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // ���ݹ��� ������ �������� ��ƼŬ ȿ�� ���
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        // �ǰ� ȿ���� ���
        enemyAudioPlayer.PlayOneShot(hitSound);

        // livingThing�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        // livingThing�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        // �ٸ� AI �������� �ʵ��� �ڽ��� ��� �ݶ��̴� ��Ȱ��ȭ
        Collider[] colls = GetComponents<Collider>(); // ���� �� �����ö��� GetComponents<>(); ���
        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = false;
        }

        // AI ���� ����, ����޽� ������Ʈ ��Ȱ��ȭ
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // ��� �ִϸ��̼� ���
        enemyAnimator.SetTrigger("Die");
        // ��� ȿ���� ���
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other) // stay: 1�ʿ� 20��(fixtoupdate: 0.2�ʿ� 1������ ����)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (!dead && Time.time >= lastAttackTime + timeBetAttack) // �ֱ� ���� �������� timeBetAttack �̻� �ð��� �����ٸ� ���� ����
        {
            Livingthing e = other.GetComponent<Livingthing>(); // ������ livingThing Ÿ�� �������� �õ�

            if (e != null && e == livingThing)
            {
                lastAttackTime = Time.time;

                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���, Raycast ����ϸ� �� �κ��� �ʿ� ����
                Vector3 hitPoint = other.ClosestPoint(transform.position); // ClosestPoint(): �÷��̾��� �ݶ��̴��� ���� �ݶ��̴� �浹 �� �ε��� ���� ����� ����, ()�ȿ� ��ü ��ġ ����
                Vector3 hitNormal = transform.position - other.transform.position; // ���� ������ �������� �ٶ󺸰� ��

                // ���� ����
                e.OnDamage(damage, hitPoint, hitNormal); // �÷��̾ ���� ����
            }
        }
    }
}