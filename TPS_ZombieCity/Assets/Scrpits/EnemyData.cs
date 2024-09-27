using UnityEngine;

// 적 생성시 사용할 셋업 데이터
[CreateAssetMenu(menuName = "Scriptable/EnemyData", fileName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    // float만 인식됨, int는 왜 인식 안 되지?

    public float health = 100f; // 체력
    public float damage = 20f; // 공격력
    public float speed = 2f; // 이동 속도
    public float killscore = 100; // 사살 시 점수
    public Color Outlinecolor; // 테두리 색깔
    public float Outlinethick; // 테두리 두깨
}