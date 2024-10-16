using System;
using UnityEngine;

public class Livingthing : MonoBehaviour, InterfaceDamage
{
    public float startHealth = 100f; // 시작 체력
    public float health { get; protected set; } // 현재 체력
    public bool dead { get; protected set; } // 사망 상태
    public event Action ActionDeath; // 사망시 발동할 이벤트, Action 타입: deligate 타입 중 매개 변수가 없음, deligate 타입: 변수에 함수들 넣음(함수 포인터 개념)->변수명() 호출시 변수 내 함수들 모두 호출 가능, event 키워드: 호출은 해당 클래스에서만 함

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable() // 실질적인 게임 오브젝트x->awake 사용x
    {
        // 사망하지 않은 상태로 시작
        dead = false;
        // 체력을 시작 체력으로 초기화
        health = startHealth;
    }

    // 데미지를 입는 기능
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        // 데미지만큼 체력 감소
        health -= damage;
        // Debug.Log(health);
        // 체력이 0 이하 && 아직 죽지 않았다면 사망 처리 실행
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 체력을 회복하는 기능
    public virtual void HealHealth(float itemHealth)
    {
        if (dead)
        {
            // 이미 사망한 경우 체력을 회복할 수 없음
            return;
        }

        // 체력 추가
        health += itemHealth;
    }

    // 사망 처리
    public virtual void Die()
    {
        // onDeath 이벤트에 등록된 메서드가 있다면 실행
        if (ActionDeath != null)
        {
            ActionDeath();
        }

        // 사망 상태를 참으로 변경
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
