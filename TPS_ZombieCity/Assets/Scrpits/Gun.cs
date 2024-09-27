using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State // enum: �̸��� ������ ��� ���� �� ��Ÿ��
    {
        Ready, // �߻� �غ�
        Empty, // ź������ ź�� ����
        Reloading // ������ ���� ��
    }

    public State state{  get; private set; }

    // public Transform[] shotPosition; // ź�� �߻� ��ġ
    public GameObject[] bulletLineposition; // ź�� �߻� ��ġ
    public GameObject[] targetPosition; // �Ѿ� ���� ��ġ

    public ParticleSystem shotFlash; // �߻� �� �ѱ� ȭ�� ȿ��

    // private List<LineRenderer> bulletLine = new List<LineRenderer>();// ź ���� �׸��� ���� ������

    private AudioSource gunSoundPlayer; // �� �Ҹ� ���
    public LineRenderer[] bulletline; // ź ���� ���η�����(�� ���� ������)
    // public LineRenderer bulletline; // ź ���� ���η�����(�Ѿ� ��ġ ���� ������)

    public GunData gunData;

    private float fireDistance = 50f; // �����Ÿ�

    public int ammoRemain; // ���� ��ü ź��
    public int magAmmo; // ���� ź������ ���� �ִ� ź��

    private float lastFireTime; // ���� ���������� �߻��� ����

    // ����ĳ��Ʈ�� ���� �浹 ���� �����ϴ� �����̳�
    private RaycastHit[] hit = new RaycastHit[9];
    // ź���� ���� ���� ������ ����
    private Vector3[] hitPosition = new Vector3[9]; // = Vector3.zero;


    // Start is called before the first frame update

    private void Awake()
    {
        // ����� ������Ʈ�� ���� ��������

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
        for (int i = 0; i < targetPosition.Length; i++)
        {
            // hitPosition[i] = Vector3.zero;
            // ����ĳ��Ʈ(���� ����, ����, �浹 ���� �����̳�, �����Ÿ�)
            if (Physics.Raycast(bulletLineposition[i].transform.position, targetPosition[i].transform.position - bulletLineposition[i].transform.position, out hit[i], fireDistance))
            {
                // ���̰� � ��ü�� �浹�� ��� �浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. ���� �ص� ��

                // �������κ��� IDamageable ������Ʈ �������� �� �����ߴٸ�
                if (target != null)
                {
                    // ������  OnDamage �Լ� ������� ���濡�� ����� �ֱ�
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // ���̰� �浹�� ��ġ ����
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�, ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
                hitPosition[i] = bulletLineposition[i].transform.position + (targetPosition[i].transform.position - bulletLineposition[i].transform.position) * fireDistance;
            }
            /*
            if (Physics.Raycast(bulletLineposition[i].transform.position, bulletLineposition[i].transform.forward, out hit[i], fireDistance))
            {
                // ���̰� � ��ü�� �浹�� ��� �浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. ���� �ص� ��

                // �������κ��� IDamageable ������Ʈ �������� �� �����ߴٸ�
                if (target != null)
                {
                    // ������  OnDamage �Լ� ������� ���濡�� ����� �ֱ�
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // ���̰� �浹�� ��ġ ����
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�, ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
                hitPosition[i] = bulletLineposition[i].transform.position + bulletLineposition[i].transform.forward * fireDistance;
            }
            */

            
            bulletline[i].SetPosition(0, bulletLineposition[i].transform.position);
            bulletline[i].SetPosition(1, hitPosition[i]);

            // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
            bulletline[i].enabled = true;

            // �߻� ����Ʈ ��� ����

            StartCoroutine(ShotEffect(hitPosition[i]));
        }


        /*
        for (int i = 0; i < shotPosition.Length; i++)
        {
            // hitPosition[i] = Vector3.zero;
            // ����ĳ��Ʈ(���� ����, ����, �浹 ���� �����̳�, �����Ÿ�)
            if (Physics.Raycast(shotPosition[i].position, shotPosition[i].forward, out hit[i], fireDistance))
            {
                // ���̰� � ��ü�� �浹�� ��� �浹�� �������κ��� IDamageable ������Ʈ �������� �õ�
                InterfaceDamage target = hit[i].collider.GetComponent<InterfaceDamage>(); // collider.gameobject. ���� �ص� ��

                // �������κ��� IDamageable ������Ʈ �������� �� �����ߴٸ�
                if (target != null)
                {
                    // ������  OnDamage �Լ� ������� ���濡�� ����� �ֱ�
                    target.OnDamage(gunData.damage, hit[i].point, hit[i].normal);
                }

                // ���̰� �浹�� ��ġ ����
                hitPosition[i] = hit[i].point;
            }
            else
            {
                // ���̰� �ٸ� ��ü�� �浹���� �ʾҴٸ�, ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
                hitPosition[i] = shotPosition[i].position + shotPosition[i].forward * fireDistance;
            }
            // �߻� ����Ʈ ��� ����
            StartCoroutine(ShotEffect(hitPosition[i]));
        }
        */

        shotFlash.Play(); // ��ƼŬ: Play()�� ���
        gunSoundPlayer.PlayOneShot(gunData.shotSoundClip); // PlayOneShot()���� �ؾ� ���� �Ҹ� ���ÿ� ��� ����, Play(): �̹� ��� ���� ������� ������ ���� �Ҹ� ������Ű�� ���� �����ϴ� �Ҹ� ���

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
        // 0.03�� ���� ��� ó���� ���, �ڷ�ƾ�� �߰������� ���ư��� ��ƾ(���α׷��� ��� ����), 0.03�� �ִٰ� �Լ� ���� �� �ٽ� �ڷ�ƾ���� ���ƿ�, �ڷ�ƾ ��� ���� ����->���� �����ϴ��� ��� �˻�
        yield return new WaitForSeconds(0.03f); // 0.03f

        for (int i = 0; i < bulletline.Length; i++)
        {
            // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
            bulletline[i].enabled = false;
        }




        // While�� �ȿ� �ڷ�ƾ �ֱ� or �ڷ�ƾ �ȿ� while�� �ֱ�->�ڷ�ƾ �ݺ� ���� ����




        /*
        for (int i = 0; i < shotPosition.Length; i++)
        {
            bulletline.SetPosition(0, shotPosition[i].position);
            bulletline.SetPosition(1, hitPosition);

            // ���� �������� Ȱ��ȭ�Ͽ� ź�� ������ �׸�
            bulletline.enabled = true;

            // 0.03�� ���� ��� ó���� ���, �ڷ�ƾ�� �߰������� ���ư��� ��ƾ(���α׷��� ��� ����), 0.03�� �ִٰ� �Լ� ���� �� �ٽ� �ڷ�ƾ���� ���ƿ�, �ڷ�ƾ ��� ���� ����->���� �����ϴ��� ��� �˻�
            yield return new WaitForSeconds(0.03f);

            // ���� �������� ��Ȱ��ȭ�Ͽ� ź�� ������ ����
            bulletline.enabled = false;

            // While�� �ȿ� �ڷ�ƾ �ֱ� or �ڷ�ƾ �ȿ� while�� �ֱ�->�ڷ�ƾ �ݺ� ���� ����

        }
        */


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
