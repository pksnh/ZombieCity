using UnityEngine;
using UnityEngine.UI; // UI ���� �ڵ�
public class PlayerHealth : Livingthing
{
    public Slider healthSlider; // ü���� ǥ���� UI �����̴�

    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public AudioClip itemPickupClip; // ������ ���� �Ҹ�

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    private PlayerAction playerAction; // �÷��̾� ������ ������Ʈ
    private Shooter Shooter; // �÷��̾� ���� ������Ʈ

    private void Awake()
    {
        // ����� ������Ʈ�� �������� �� �ʱ�ȭ->OnEnable()���� ������Ʈ�� setactive(true)�ϴ� ��� ���������� ����� �� �ֵ��� ��
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();

        playerAction = GetComponent<PlayerAction>(); // �÷��̾� ��� �� ��Ȱ��ȭ �ϱ� ����
        Shooter = GetComponent<Shooter>(); // �÷��̾� ��� �� ��Ȱ��ȭ �ϱ� ����
    }

    protected override void OnEnable()
    {
        // livingThing�� OnEnable() ���� (���� �ʱ�ȭ), livingThing�� ������ �� �ִ� ���� livingThing������ ������ ��->�ڽ� Ŭ���������� �ִ��� �Լ��� ����� ��
        base.OnEnable(); // �θ� Ŭ���� �� ���� �ϰ� �ڽ� Ŭ���� �� �ۼ��� ��

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = health;

        playerAction.enabled = true;
        Shooter.enabled = true;

    }

    // ü�� ȸ��

    public override void HealHealth(float ChangeHealth)
    {
        // livingThing�� RestoreHealth() ���� (ü�� ����)
        base.HealHealth(ChangeHealth);

        healthSlider.value = health;
    }

    // ������ ó��

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }
        // livingThing�� OnDamage() ����(������ ����)
        base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health;
    }

    // ��� ó��

    public override void Die()
    {
        // livingThing�� Die() ����(��� ����)
        base.Die();

        healthSlider.gameObject.SetActive(false);

        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerAction.enabled = false;
        Shooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����۰� �浹�� ��� �ش� �������� ����ϴ� ó��
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
