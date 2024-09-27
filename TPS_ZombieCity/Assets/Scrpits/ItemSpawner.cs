using UnityEngine;
using UnityEngine.AI; // ����޽� ���� �ڵ�

// �ֱ������� �������� �÷��̾� ��ó�� �����ϴ� ��ũ��Ʈ-�ڵ� �߻�ȭ(�ڵ� ��ü�� �� �ٷ� ���)
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // ������ �����۵�
    public Transform playerTransform; // �÷��̾��� Ʈ������

    public float maxDistance = 5f; // �÷��̾� ��ġ�κ��� �������� ��ġ�� �ִ� �ݰ�(5m �ݰ� ��)

    public float timeBetSpawnMax = 7f; // �ִ� �ð� ����
    public float timeBetSpawnMin = 2f; // �ּ� �ð� ����
    private float timeBetSpawn; // ���� ����

    private float lastSpawnTime; // ������ ���� ����

    private void Start()
    {
        // ���� ���ݰ� ������ ���� ���� �ʱ�ȭ
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // �ֱ������� ������ ���� ó�� ����
    private void Update()
    {
        // ���� ������ ������ ���� �������� ���� �ֱ� �̻� ����
        // && �÷��̾� ĳ���Ͱ� ������
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            // ������ ���� �ð� ����
            lastSpawnTime = Time.time;
            // ���� �ֱ⸦ �������� ����
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // ������ ���� ����
            Spawn();
        }
    }

    // ���� ������ ���� ó��
    private void Spawn()
    {
        // �÷��̾� ��ó���� ����޽� ���� ���� ��ġ ��������(maxDistnace ���� �ȿ���) 
        Vector3 spawnPosition =
            GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        // �ٴڿ��� 5��ŭ ���� �ø���
        spawnPosition += Vector3.up * 1.0f; // == spawnPosition.y += 1.0f;, spawnPosition = 1.0f;-�̰� ���� ��� �ƴ�(����޽� ��ġ ���� �� y�� 0�̶�� ������ ����)

        // ������ �� �ϳ��� �������� ��� ���� ��ġ�� ����
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity); // item: Instantiate�� ���� �ν��Ͻ��� �ı��ϱ� ���� �ν��Ͻ� ���� ����

        // ������ �������� 5�� �ڿ� �ı�, ��� �������� �����ϸ� �ʿ� ������ ��ó��+���ӿ� ���尨 �ο�(5�� ���� �� ������ ������ �����)��
        Destroy(item, 5f);
    }

    // ����޽� ���� ������ ��ġ�� ��ȯ�ϴ� �޼���
    // center�� �߽����� distance �ݰ� �ȿ��� ������ ��ġ�� ã�´�
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center(�÷��̾� ��ġ)�� �߽����� �������� maxDistance�� �� �ȿ����� ������ ��ġ �ϳ��� ����
        // Random.insideUnitSphere�� �������� 1�� �� �ȿ����� ������ �� ���� ��ȯ�ϴ� ������Ƽ
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // ����޽� ���ø��� ��� ������ �����ϴ� ����
        NavMeshHit hit;

        // maxDistance �ݰ� �ȿ���, randomPos�� ���� ����� ����޽� ���� �� ���� ã��, ���� ��ġ �� ��ġ �ϳ��� ����(sampling)
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas); // static �Լ�(=Ŭ���� �Լ�)

        // ã�� �� ��ȯ
        return hit.position;
    }
}