using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gun1 : MonoBehaviour
{
    public enum State // enum: �̸��� ������ ��� ���� �� ��Ÿ��
    {
        Ready, // �߻� �غ�
        Empty, // ź������ ź�� ����
        Reloading // ������ ���� ��
    }

    public State state{  get; private set; }

    public Transform shotPosition; // ź�� �߻� ��ġ
    // public List<GameObject> bulletLineposition = new List<GameObject>();

    public ParticleSystem shotFlash; // �߻� �� �ѱ� ȭ�� ȿ��

    // private List<LineRenderer> bulletLine = new List<LineRenderer>();// ź ���� �׸��� ���� ������

    private AudioSource gunSoundPlayer; // �� �Ҹ� ���
    private LineRenderer bulletline; // ź ���� ���η�����

    public GunData gunData;

    private float fireDistance = 50f; // �����Ÿ�

    public int ammoRemain = 20; // ���� ��ü ź��
    public int magAmmo = 2; // ���� ź������ ���� �ִ� ź��

    private float lastFireTime; // ���� ���������� �߻��� ����


    // Start is called before the first frame update

    private void Awake()
    {
        // ����� ������Ʈ�� ���� ��������

        gunSoundPlayer = GetComponent<AudioSource>();
        bulletline = GetComponent<LineRenderer>();

        bulletline.positionCount = 2;
        bulletline.enabled = false;

        // �� ���� �ʱ�ȭ
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
        // ���� ���°� �߻� ������ ���� && ������ �� �߻� �������� timeBetFire �̻��� �ð� ����
        if (this.isActiveAndEnabled == true && state == State.Ready && Time.time >= lastFireTime + gunData.timeBetShot && magAmmo > 0)
        {
            // ������ �� �߻� ���� ����
            lastFireTime = Time.time;
            // ���� �߻� ó�� ����
            Shot();
        }
    }

    public void Shot()
    {
        // ����ĳ��Ʈ�� ���� �浹 ���� �����ϴ� �����̳�
        RaycastHit hit;
        // ź���� ���� ���� ������ ����
        Vector3 hitPosition = Vector3.zero;

        // hitPosition[i] = Vector3.zero;
        // ����ĳ��Ʈ(���� ����, ����, �浹 ���� �����̳�, �����Ÿ�)
        if (Physics.Raycast(shotPosition.position, shotPosition.forward, out hit, fireDistance))
        {
            // ���̰� � ��ü�� �浹�� ��� �浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
            InterfaceDamage target = hit.collider.GetComponent<InterfaceDamage>(); // collider.gameobject. ���� �ص� ��

            // �������κ��� IDamageable ������Ʈ �������� �� �����ߴٸ�
            if (target != null)
            {
                // ������  OnDamage �Լ� ������� ���濡�� ����� �ֱ�
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;
        }
        else
        {
            // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�, ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = shotPosition.position + shotPosition.forward * fireDistance;
        }

        // �߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitPosition));

        // ���� ź�� ���� -1
        magAmmo--;
        if (magAmmo <= 0)
        {
            // źâ�� ���� ź���� ���ٸ� ���� ���� ���¸� Empty�� ����
            state = State.Empty;
        }
 
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition) // �ڷ�ƾ: ���� ������ �����ؼ� ����, �ڷ�ƾ �������� �ڷ�ƾ ��� ���ߴ� ���� ���� ������ ��� ����
    {
        shotFlash.Play(); // ��ƼŬ: Play()�� ���
        gunSoundPlayer.PlayOneShot(gunData.shotSoundClip); // PlayOneShot()���� �ؾ� ���� �Ҹ� ���ÿ� ��� ����, Play(): �̹� ��� ���� ������� ������ ���� �Ҹ� ������Ű�� ���� �����ϴ� �Ҹ� ���

        bulletline.SetPosition(0, shotPosition.position);
        bulletline.SetPosition(1, hitPosition);

        // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
        bulletline.enabled = true;

        // 0.03�� ���� ��� ó���� ���, �ڷ�ƾ�� �߰������� ���ư��� ��ƾ(���α׷��� ��� ����), 0.03�� �ִٰ� �Լ� ���� �� �ٽ� �ڷ�ƾ���� ���ƿ�, �ڷ�ƾ ��� ���� ����->���� �����ϴ��� ��� �˻�
        yield return new WaitForSeconds(0.03f);

        // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
        bulletline.enabled = false;

        // While�� �ȿ� �ڷ�ƾ �ֱ� or �ڷ�ƾ �ȿ� while�� �ֱ�->�ڷ�ƾ �ݺ� ���� ����


    }

    public void Reload()
    {
        if (state != State.Reloading && ammoRemain > 0  && magAmmo < gunData.magCapacity)
        {
            // if (state == State.Reloading || ammoRemain <= 0  || magAmmo >= gunData.magCapacity)
            // �̹� ������ ���̰ų� ���� ź���� ���ų� źâ�� ź���� ������ ��쳪 źâ �뷮���� �� ���� ��� �������� �� ����
            // return false; // �ܼ��� ���� �ǹ�x, ������ ���� ���� ���� �Ǵ� ����
            
            // ������ ó�� ����
            StartCoroutine(ReloadRoutine());
        }

        

    }

    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        // ���� ���¸� ������ �� ���·� ��ȯ->������ �߿� �ٽ� ������ �õ� ����
        state = State.Reloading;

        gunSoundPlayer.PlayOneShot(gunData.reloadSoundClip);

        
        int ammoToFill = gunData.magCapacity - magAmmo;

        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;

        // ������ �ҿ� �ð� ��ŭ ó�� ����
        yield return new WaitForSeconds(gunData.reloadTime);

        // ������ �ð� ���� �� ź�� ���� ����

        // ���� ���� ���¸� �߻� �غ�� ���·� ����
        state = State.Ready;
    }
}
