using System.Collections.Generic;
using UnityEngine;

// ���� ���� ������Ʈ�� �ֱ������� ����
public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs; // ������ ���� ���� ������

    public EnemyData[] enemyDatas; // ����� ���� �¾� �����͵�
    public Transform[] spawnPoints; // ���� AI�� ��ȯ�� ��ġ��

    private List<Enemy> enemys = new List<Enemy>(); // ������ ������� ��� ����Ʈ
    private int wave; // ���� ���̺�

    private void Update()
    {
        // ���� ���� �����϶��� �������� ����
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // ���� ��� ����ģ ��� ���� ���� ����
        if (enemys.Count <= 0)
        {
            SpawnWave();
        }

        // UI ����
        UpdateUI();
    }

    // ���̺� ������ UI�� ǥ��
    private void UpdateUI()
    {
        // ���� ���̺�� ���� �� �� ǥ��
        UIManager.instance.UpdateWaveText(wave, enemys.Count);
    }

    // ���� ���̺꿡 ���� ������� ����
    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt((wave*10) * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateEnemy();
        }
    }

    // ���� �����ϰ� ������ ���񿡰� ������ ����� �Ҵ�
    private void CreateEnemy()
    {
        Enemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        EnemyData enemyData = enemyDatas[Random.Range(0, enemyDatas.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(enemyData);
        enemys.Add(enemy);

        enemy.ActionDeath += () => enemys.Remove(enemy); // () => zombies.Remove(zombie);- �͸� �Լ�, ���� ���� ����Ʈ���� ��
        enemy.ActionDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.ActionDeath += () => GameManager.instance.AddScore((int)enemyData.killscore);
        enemy.ActionDeath += () => GameManager.instance.AddKillCount(1);
    }
}