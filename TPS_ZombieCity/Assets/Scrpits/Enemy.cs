using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, 내비게이션 시스템 관련 코드 가져오기

// 좀비 AI 구현
public class Enemy : Livingthing
{
    public LayerMask whatIsTarget; // 추적 대상 레이어, 레이어: 일종의 그룹핑, 마스크: 선택적으로 걸러내는 역할

    private Livingthing livingThing; // 추적 대상, Livingthing 상속 받는 대상 추적, 플레이어 추적+좀비 군집이동
    private NavMeshAgent navMeshAgent; // 경로 계산 AI 에이전트, UnityEngine.AI 필요, zombie 이동시킴

    public ParticleSystem hitEffect; // 피격 시 재생할 파티클 효과
    public AudioClip deathSound; // 사망 시 재생할 소리
    public AudioClip hitSound; // 피격 시 재생할 소리
    // public EnemyData enemyData; // 적 스크립터블 데이터


    private Animator enemyAnimator; // 애니메이터 컴포넌트
    private AudioSource enemyAudioPlayer; // 오디오 소스 컴포넌트
    // private Renderer enemyRenderer; // 렌더러 컴포넌트
    private Outline outLine;

    public float damage = 20f; // 공격력
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시점

    // 추적할 대상이 존재하는지 알려주는 프로퍼티
    private bool tryTargeting // set 없음->=으로 값 대입 불가
    {
        get
        {
            // 추적할 대상이 존재하고, 대상이 사망하지 않았다면 true
            if (livingThing != null && !livingThing.dead)
            {
                return true;
            }

            // 그렇지 않다면 false
            return false;
        }
    }

    private void Awake()
    {
        // 초기화
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        // enemyRenderer = GetComponentInChildren<Renderer>();
        outLine = GetComponentInChildren<Outline>();
    }

    // 좀비 AI의 초기 스펙을 결정하는 셋업 메서드
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
        // 게임 오브젝트 활성화와 동시에 AI의 추적 루틴 시작
        StartCoroutine(UpdatePath()); // StartCoroutine("UpdatePath");와 동일, 보통 코루틴 함수는 start()에 선언함
    }

    private void Update()
    {
        // 추적 대상의 존재 여부에 따라 다른 애니메이션 재생
        enemyAnimator.SetBool("Targeting", tryTargeting);
    }

    // 주기적으로 추적할 대상의 위치를 찾아 경로 갱신
    private IEnumerator UpdatePath()
    {
        // 살아 있는 동안 무한 루프, 죽으면 종료
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

                Collider[] colliders = Physics.OverlapSphere(transform.position, 200f, whatIsTarget); // 좀비 위치에서 시작해서 반지름 500m 구 안에서 whatIsTarget 추적, overlapSphere 추후에도 잘 씀

                for (int i = 0; i < colliders.Length; i++)
                {
                    Livingthing e = colliders[i].GetComponent<Livingthing>();
                    if (e != null && !e.dead)
                    {
                        livingThing = e; // livingThing에 추적할 대상 넣음
                        break;
                    }
                }
            }
            // 0.25초 주기로 처리 반복
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 데미지를 입었을 때 실행할 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 공격받은 지점과 방향으로 파티클 효과 재상
        if (!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }

        // 피격 효과음 재생
        enemyAudioPlayer.PlayOneShot(hitSound);

        // livingThing의 OnDamage()를 실행하여 데미지 적용
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // 사망 처리
    public override void Die()
    {
        // livingThing의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();

        // 다른 AI 방해하지 않도록 자신의 모든 콜라이더 비활성화
        Collider[] colls = GetComponents<Collider>(); // 여러 개 가져올때는 GetComponents<>(); 사용
        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = false;
        }

        // AI 추적 중지, 내비메시 컴포넌트 비활성화
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        // 사망 애니메이션 재생
        enemyAnimator.SetTrigger("Die");
        // 사망 효과음 재생
        enemyAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other) // stay: 1초에 20번(fixtoupdate: 0.2초에 1번으로 설정)
    {
        // 트리거 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        if (!dead && Time.time >= lastAttackTime + timeBetAttack) // 최근 공격 시점에서 timeBetAttack 이상 시간이 지났다면 공격 가능
        {
            Livingthing e = other.GetComponent<Livingthing>(); // 상대방의 livingThing 타입 가져오기 시도

            if (e != null && e == livingThing)
            {
                lastAttackTime = Time.time;

                // 상대방의 피격 위치와 피격 방향을 근삿값으로 계산, Raycast 사용하면 이 부분은 필요 없음
                Vector3 hitPoint = other.ClosestPoint(transform.position); // ClosestPoint(): 플레이어의 콜라이더와 공격 콜라이더 충돌 시 부딪힌 가장 가까운 지점, ()안에 주체 위치 넣음
                Vector3 hitNormal = transform.position - other.transform.position; // 적이 공격한 방향으로 바라보게 함

                // 공격 실행
                e.OnDamage(damage, hitPoint, hitNormal); // 플레이어가 공격 당함
            }
        }
    }
}