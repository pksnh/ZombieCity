using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Gun gun; // ����� ��
    public Gun1 gun1; // ����� ��
    public Transform gunPosition; // �� ��ġ ������
    public Transform leftHand; // �� ��� �޼��� ��ġ�� ����
    public Transform rightHand; // �� ��� �������� ��ġ�� ����
    // public GameObject gun1BlackImage;

    private PlayerActionInput playerActionInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        playerActionInput = GetComponent<PlayerActionInput>();
        playerAnimator = GetComponent<Animator>();

        gun.gameObject.SetActive(true);
        gun1.gameObject.SetActive(true);
        gun1.gameObject.SetActive(false);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
        gun1.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGun1UI();
        UpdateGun2UI();
    }

    private void UpdateGun1UI()
    {
        if (gun != null && UIManager.instance != null)
        {
            // UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź��� ���� ��ü ź���� ǥ��, instance ȣ�� �� instance get �۵�->�׷��� if���� UIManager.instance != null �ʿ� ����� ��
            UIManager.instance.UpdateGun1AmmoText(gun.magAmmo, gun.ammoRemain);
        } 
    }

    private void UpdateGun2UI()
    {
        if (gun1 != null && UIManager.instance != null)
        {
            // UI �Ŵ����� ź�� �ؽ�Ʈ�� źâ�� ź��� ���� ��ü ź���� ǥ��, instance ȣ�� �� instance get �۵�->�׷��� if���� UIManager.instance != null �ʿ� ����� ��
            UIManager.instance.UpdateGun2AmmoText(gun1.magAmmo, gun1.ammoRemain);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // ���� ������ gunPivot�� 3D ���� ������ �Ȳ�ġ ��ġ�� �̵�, GetIKHintPosition(): IK ��ġ�� �޾ƿ�
        gunPosition.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK�� ����Ͽ� �޼��� ��ġ�� ȸ���� ���� ������ �����̿� �����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); // Weight: �� IK ���� ���� ���� ���� ǥ��(1.0f: �� ���� ���� 100% ����), ����ġ ���� �ǹ�, �� ���� ������ ���̴� ��� 1.0f�� �� ��(�⺻������ 1.0f���� �� ��)
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position); // LeftHand�� leftHandMOunt�� ��ġ�� ����
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);

        // IK�� ����Ͽ� �������� ��ġ�� ȸ���� ���� ������ �����̿� �����
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
    }
}
