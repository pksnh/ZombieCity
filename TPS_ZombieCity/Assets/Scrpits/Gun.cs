using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State // enum: 이름이 지정된 상수 집합 값 나타냄
    {
        Ready, // 발사 준비
        Empty, // 탄알집에 탄알 없음
        Reloading // 재장전 동작 중
    }

    public State state{  get; private set; }

    // public Transform[] shotPosition; // 탄알 발사 위치
    public GameObject[] bulletLineposition; // 탄알 발사 위치
    public GameObject[] targetPosition; // 총알 도착 위치

    public ParticleSystem shotFlash; // 발사 시 총구 화염 효과

    // private List<LineRenderer> bulletLine = new List<LineRenderer>();// 탄 궤적 그리는 라인 렌더러

    private AudioSource gunSoundPlayer; // 총 소리 재생
    public LineRenderer[] bulletline; // 탄 궤적 라인렌더러(총 라인 렌더러)
    // public LineRenderer bulletline; // 탄 궤적 라인렌더러(총알 위치 라인 렌더러)

    public GunData gunData;

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    // 레이캐스트에 의한 충돌 정보 저장하는 컨테이너
    private RaycastHit[] hit = new RaycastHit[9];
    // 탄알이 맞은 곳을 저장할 변수
    private Vector3[] hitPosition = new Vector3[9]; // = Vector3.zero;


    // Start is called before the first frame update

    private void Awake()
    {
        // 사용할 컴포넌트의 참조 가져오기

        gunSoundPlayer = GetComponent<AudioSource>();

        for(int i = 0; i < bulletLineposition.Length; i++)
        {
            bulletline[i].positionCount = 2;
            bulletline[i].enabled = false;
        }



        // for (int i = 0; i < targetPosition.Length; i++) { }
        // bulletline[i] = bulletLineposition[i].GetComponent<LineRenderer>();
        // bulletline.positionCount = 2;
        // bulletline.enabled = false;
        
        

        // 총 상태 초기화
        ammoRemain = gunData.startAmmo;
        magAmmo = gunData.magCapacity;

        lastFireTime = 0;
    }

    private void OnEnable()
    {


        state = State.Ready;
        
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Fire()
    {
        // 현재 상태가 발사 가능한 상태 && 마지막 총 발사 시점에서 timeBetFire 이상의 시간 지남
        if (this.isActiveAndEnabled == true && state == State.Ready && Time.time >= lastFireTime + gunData.timeBetShot && magAmmo > 0)
        {
            // 마지막 총 발사 시점 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot();
        }
    }

    public void Shot()
    {
        for (int i = 0; i < targetPosition.Length; i++)
        {
            // hitPosition[i] = Vector3.zero;
            // 레이캐스트(시작 지점, 방향, 충돌 정보 컨테이너, 사정거리)
            if (Physics.Raycast(bulletLineposition[i].transform.position, targetPosition[i].transform.position - bulletLineposition[i].transform.position, out hit[i], fireDistance))
            {
                // 레이가 어떤 물체와 충돌한 경우 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. 으로 해도 됨

                // 상대방으로부터 IDamageable 오브젝트 가져오는 데 성공했다면
                if (target != null)
                {
                    // 상대방의  OnDamage 함수 실행시켜 상대방에게 대미지 주기
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // 레이가 충돌한 위치 저장
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // 레이가 다른 물체와 충돌하지 않았다면, 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
                hitPosition[i] = bulletLineposition[i].transform.position + (targetPosition[i].transform.position - bulletLineposition[i].transform.position) * fireDistance;
            }
            /*
            if (Physics.Raycast(bulletLineposition[i].transform.position, bulletLineposition[i].transform.forward, out hit[i], fireDistance))
            {
                // 레이가 어떤 물체와 충돌한 경우 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. 으로 해도 됨

                // 상대방으로부터 IDamageable 오브젝트 가져오는 데 성공했다면
                if (target != null)
                {
                    // 상대방의  OnDamage 함수 실행시켜 상대방에게 대미지 주기
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // 레이가 충돌한 위치 저장
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // 레이가 다른 물체와 충돌하지 않았다면, 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
                hitPosition[i] = bulletLineposition[i].transform.position + bulletLineposition[i].transform.forward * fireDistance;
            }
            */

            
            bulletline[i].SetPosition(0, bulletLineposition[i].transform.position);
            bulletline[i].SetPosition(1, hitPosition[i]);

            // 라인 렌더러를 활성화하여 탄알 궤적을 그림
            bulletline[i].enabled = true;

            // 발사 이팩트 재생 시작

            StartCoroutine(ShotEffect(hitPosition[i]));
        }


        /*
        for (int i = 0; i < shotPosition.Length; i++)
        {
            // hitPosition[i] = Vector3.zero;
            // 레이캐스트(시작 지점, 방향, 충돌 정보 컨테이너, 사정거리)
            if (Physics.Raycast(shotPosition[i].position, shotPosition[i].forward, out hit[i], fireDistance))
            {
                // 레이가 어떤 물체와 충돌한 경우 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. 으로 해도 됨

                // 상대방으로부터 IDamageable 오브젝트 가져오는 데 성공했다면
                if (target != null)
                {
                    // 상대방의  OnDamage 함수 실행시켜 상대방에게 대미지 주기
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // 레이가 충돌한 위치 저장
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // 레이가 다른 물체와 충돌하지 않았다면, 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
                hitPosition[i] = shotPosition[i].position + shotPosition[i].forward * fireDistance;
            }
            // 발사 이팩트 재생 시작
            StartCoroutine(ShotEffect(hitPosition[i]));
        }
        */

        shotFlash.Play(); // 파티클: Play()로 재생
        gunSoundPlayer.PlayOneShot(gunData.shotSoundClip); // PlayOneShot()으로 해야 여러 소리 동시에 재생 가능, Play(): 이미 재생 중인 오디오가 있으면 기존 소리 정지시키고 현재 동작하는 소리 재생

        // 남은 탄알 수를 -1

        magAmmo--;

        if (magAmmo <= 0)
        {
            // 탄창에 남은 탄알이 없다면 총의 현재 상태를 Empty로 갱신
            state = State.Empty;
        }
 
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) // 코루틴: 기존 로직과 병행해서 실행, 코루틴 실행으로 코루틴 잠시 멈추는 동안 기존 로직은 계속 실행
    {
        // 0.03초 동안 잠시 처리를 대기, 코루틴은 추가적으로 돌아가는 루틴(프로그램은 계속 진행), 0.03초 있다가 함수 실행 후 다시 코루틴으로 돌아옴, 코루틴 목록 따로 지정->조건 만족하는지 계속 검사
        yield return new WaitForSeconds(0.03f); // 0.03f

        for (int i = 0; i < bulletline.Length; i++)
        {
            // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
            bulletline[i].enabled = false;
        }




        // While문 안에 코루틴 넣기 or 코루틴 안에 while문 넣기->코루틴 반복 실행 가능




        /*
        for (int i = 0; i < shotPosition.Length; i++)
        {
            bulletline.SetPosition(0, shotPosition[i].position);
            bulletline.SetPosition(1, hitPosition);

            // 라인 렌더러를 활성화하여 탄알 궤적을 그림
            bulletline.enabled = true;

            // 0.03초 동안 잠시 처리를 대기, 코루틴은 추가적으로 돌아가는 루틴(프로그램은 계속 진행), 0.03초 있다가 함수 실행 후 다시 코루틴으로 돌아옴, 코루틴 목록 따로 지정->조건 만족하는지 계속 검사
            yield return new WaitForSeconds(0.03f);

            // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
            bulletline.enabled = false;

            // While문 안에 코루틴 넣기 or 코루틴 안에 while문 넣기->코루틴 반복 실행 가능

        }
        */


    }

    public void Reload()
    {
        if (state != State.Reloading && ammoRemain > 0  && magAmmo < gunData.magCapacity)
        {
            // if (state == State.Reloading || ammoRemain <= 0  || magAmmo >= gunData.magCapacity)
            // 이미 재장전 중이거나 남은 탄알이 없거나 탄창에 탄알이 가득한 경우나 탄창 용량보다 더 많은 경우 재장전할 수 없음
            // return false; // 단순히 실패 의미x, 정해진 동작 수행 여부 판단 가능
            
            // 재장전 처리 시작
            StartCoroutine(ReloadRoutine());
        }
        

        

    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine()
    {
        // 현재 상태를 재장전 중 상태로 전환->재장전 중에 다시 재장전 시도 방지
        state = State.Reloading;

        gunSoundPlayer.PlayOneShot(gunData.reloadSoundClip);

        
        int ammoToFill = gunData.magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 재장전 시간 끝난 후 탄약 상태 변경

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
    }
}
