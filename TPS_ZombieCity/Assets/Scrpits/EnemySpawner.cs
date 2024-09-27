using System.Collections.Generic;
using UnityEngine;

// 좀비 게임 오브젝트를 주기적으로 생성
public class EnemySpawner : MonoBehaviour
{
    public Enemy[] enemyPrefabs; // 생성할 좀비 원본 프리팹

    public EnemyData[] enemyDatas; // 사용할 좀비 셋업 데이터들
    public Transform[] spawnPoints; // 좀비 AI를 소환할 위치들

    private List<Enemy> enemys = new List<Enemy>(); // 생성된 좀비들을 담는 리스트
    private int wave; // 현재 웨이브

    private void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (enemys.Count <= 0)
        {
            SpawnWave();
        }

        // UI 갱신
        UpdateUI();
    }

    // 웨이브 정보를 UI로 표시
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적 수 표시
        UIManager.instance.UpdateWaveText(wave, enemys.Count);
    }

    // 현재 웨이브에 맞춰 좀비들을 생성
    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt((wave*10) * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateEnemy();
        }
    }

    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateEnemy()
    {
        Enemy enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        EnemyData enemyData = enemyDatas[Random.Range(0, enemyDatas.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Setup(enemyData);
        enemys.Add(enemy);

        enemy.ActionDeath += () => enemys.Remove(enemy); // () => zombies.Remove(zombie);- 익명 함수, 좀비를 좀비 리스트에서 뺌
        enemy.ActionDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.ActionDeath += () => GameManager.instance.AddScore((int)enemyData.killscore);
        enemy.ActionDeath += () => GameManager.instance.AddKillCount(1);
    }
}