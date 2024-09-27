using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")] // Attribute
public class GunData : ScriptableObject // 일반적인 클래스(MonoBehaviour 없어서 컴포넌트로 넣을 수 없음), 총 주울 때 처음 초기화할 때 한번만 사용, 초기화값 설정용이라고 볼 것, Gun 스크립트에 종속되어 있음
{
    public AudioClip shotSoundClip; // 발사 소리
    public AudioClip reloadSoundClip; // 재장전 소리

    public float damage; // 공격력

    public int startAmmo ; // 처음에 주어질 전체 탄약, 처음 세팅할 때
    public int magCapacity; // 탄알 용량

    public float timeBetShot; // 탄알 발사 간격
    public float reloadTime; // 재장전 시간
}
