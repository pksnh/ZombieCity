using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
public class PlayerHealth : Livingthing
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerAction playerAction; // 플레이어 움직임 컴포넌트
    private Shooter Shooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기 및 초기화->OnEnable()에서 컴포넌트를 setactive(true)하는 경우 안정적으로 진행될 수 있도록 함
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerAction = GetComponent<PlayerAction>(); // 플레이어 사망 시 비활성화 하기 위함
        Shooter = GetComponent<Shooter>(); // 플레이어 사망 시 비활성화 하기 위함
    }

    protected override void OnEnable()
    {
        // livingThing의 OnEnable() 실행 (상태 초기화), livingThing의 변수에 값 넣는 것은 livingThing에서만 관리할 것->자식 클래스에서는 최대한 함수만 사용할 것
        base.OnEnable(); // 부모 클래스 것 먼저 하고 자식 클래스 것 작성할 것

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = health;

        playerAction.enabled = true;
        Shooter.enabled = true;

    }

    // 체력 회복

    public override void HealHealth(float ChangeHealth)
    {
        // livingThing의 RestoreHealth() 실행 (체력 증가)
        base.HealHealth(ChangeHealth);

        healthSlider.value = health;
    }

    // 데미지 처리

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        // livingThing의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health;
    }

    // 사망 처리

    public override void Die()
    {
        // livingThing의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerAction.enabled = false;
        Shooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            InterfaceItem item = other.GetComponent<InterfaceItem>();
            if (item != null)
            {
                item.Use(gameObject);
                playerAudioPlayer.PlayOneShot(itemPickupClip);
            }
        }
    }
}
