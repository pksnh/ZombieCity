using UnityEngine;
using UnityEngine.AI; // 내비메쉬 관련 코드

// 주기적으로 아이템을 플레이어 근처에 생성하는 스크립트-코드 추상화(코드 전체를 한 줄로 요약)
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items; // 생성할 아이템들
    public Transform playerTransform; // 플레이어의 트랜스폼

    public float maxDistance = 5f; // 플레이어 위치로부터 아이템이 배치될 최대 반경(5m 반경 안)

    public float timeBetSpawnMax = 7f; // 최대 시간 간격
    public float timeBetSpawnMin = 2f; // 최소 시간 간격
    private float timeBetSpawn; // 생성 간격

    private float lastSpawnTime; // 마지막 생성 시점

    private void Start()
    {
        // 생성 간격과 마지막 생성 시점 초기화
        timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
        lastSpawnTime = 0;
    }

    // 주기적으로 아이템 생성 처리 실행
    private void Update()
    {
        // 현재 시점이 마지막 생성 시점에서 생성 주기 이상 지남
        // && 플레이어 캐릭터가 존재함
        if (Time.time >= lastSpawnTime + timeBetSpawn && playerTransform != null)
        {
            // 마지막 생성 시간 갱신
            lastSpawnTime = Time.time;
            // 생성 주기를 랜덤으로 변경
            timeBetSpawn = Random.Range(timeBetSpawnMin, timeBetSpawnMax);
            // 아이템 생성 실행
            Spawn();
        }
    }

    // 실제 아이템 생성 처리
    private void Spawn()
    {
        // 플레이어 근처에서 내비메시 위의 랜덤 위치 가져오기(maxDistnace 범위 안에서) 
        Vector3 spawnPosition =
            GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        // 바닥에서 5만큼 위로 올리기
        spawnPosition += Vector3.up * 1.0f; // == spawnPosition.y += 1.0f;, spawnPosition = 1.0f;-이건 좋은 방법 아님(내비메쉬 위치 정보 중 y가 0이라는 보장은 없음)

        // 아이템 중 하나를 무작위로 골라 랜덤 위치에 생성
        GameObject selectedItem = items[Random.Range(0, items.Length)];
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity); // item: Instantiate로 받은 인스턴스를 파괴하기 위해 인스턴스 받은 변수

        // 생성된 아이템을 5초 뒤에 파괴, 계속 아이템이 존재하면 맵에 아이템 넘처남+게임에 긴장감 부여(5초 내에 안 먹으면 아이템 사라짐)ㄴ
        Destroy(item, 5f);
    }

    // 내비메시 위의 랜덤한 위치를 반환하는 메서드
    // center를 중심으로 distance 반경 안에서 랜덤한 위치를 찾는다
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        // center(플레이어 위치)를 중심으로 반지름이 maxDistance인 구 안에서의 랜덤한 위치 하나를 저장
        // Random.insideUnitSphere는 반지름이 1인 구 안에서의 랜덤한 한 점을 반환하는 프로퍼티
        Vector3 randomPos = Random.insideUnitSphere * distance + center;

        // 내비메시 샘플링의 결과 정보를 저장하는 변수
        NavMeshHit hit;

        // maxDistance 반경 안에서, randomPos에 가장 가까운 내비메시 위의 한 점을 찾음, 여러 위치 중 위치 하나만 추출(sampling)
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas); // static 함수(=클래스 함수)

        // 찾은 점 반환
        return hit.position;
    }
}