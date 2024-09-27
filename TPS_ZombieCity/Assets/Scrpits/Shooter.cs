using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Gun gun; // 사용할 총
    public Gun1 gun1; // 사용할 총
    public Transform gunPosition; // 총 위치 기준점
    public Transform leftHand; // 총 잡는 왼손이 위치할 지점
    public Transform rightHand; // 총 잡는 오른손이 위치할 지점
    // public GameObject gun1BlackImage;

    private PlayerActionInput playerActionInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

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
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시, instance 호출 시 instance get 작동->그래서 if에서 UIManager.instance != null 필요 없기는 함
            UIManager.instance.UpdateGun1AmmoText(gun.magAmmo, gun.ammoRemain);
        } 
    }

    private void UpdateGun2UI()
    {
        if (gun1 != null && UIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시, instance 호출 시 instance get 작동->그래서 if에서 UIManager.instance != null 필요 없기는 함
            UIManager.instance.UpdateGun2AmmoText(gun1.magAmmo, gun1.ammoRemain);
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동, GetIKHintPosition(): IK 위치값 받아옴
        gunPosition.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

        // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f); // Weight: 각 IK 점에 대한 접착 정도 표현(1.0f: 그 점에 대해 100% 붙음), 가중치 범위 의미, 한 점에 완전히 붙이는 경우 1.0f로 할 것(기본적으로 1.0f으로 할 것)
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position); // LeftHand를 leftHandMOunt의 위치에 붙임
        playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);

        // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
    }
}
