using UnityEngine;

// 총알을 충전하는 아이템
public class SlugPack : MonoBehaviour, InterfaceItem
{
    public int ammo_slug = 4; // 충전할 총알 수

    public void Use(GameObject target)
    {
        // 전달 받은 게임 오브젝트로부터 PlayerShooter 컴포넌트를 가져오기 시도
        Shooter playerShooter = target.GetComponent<Shooter>();

        // PlayerShooter 컴포넌트가 있으며, 총 오브젝트가 존재하면
        if (playerShooter != null && playerShooter.gun1 != null)
        {
            playerShooter.gun1.ammoRemain += ammo_slug;
        }

        // 사용되었으므로, 자신을 파괴
        Destroy(gameObject);
    }
}