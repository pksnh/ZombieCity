using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/GunData", fileName = "Gun Data")] // Attribute
public class GunData : ScriptableObject // �Ϲ����� Ŭ����(MonoBehaviour ��� ������Ʈ�� ���� �� ����), �� �ֿ� �� ó�� �ʱ�ȭ�� �� �ѹ��� ���, �ʱ�ȭ�� �������̶�� �� ��, Gun ��ũ��Ʈ�� ���ӵǾ� ����
{
    public AudioClip shotSoundClip; // �߻� �Ҹ�
    public AudioClip reloadSoundClip; // ������ �Ҹ�

    public float damage; // ���ݷ�

    public int startAmmo ; // ó���� �־��� ��ü ź��, ó�� ������ ��
    public int magCapacity; // ź�� �뷮

    public float timeBetShot; // ź�� �߻� ����
    public float reloadTime; // ������ �ð�
}
