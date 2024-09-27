using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    private int score = 0; // ���� ���� ����, AddScore()�θ� ���� ����
    private int bestscore; // ���� �� �ְ� ���� ǥ��
    private int killcount = 0; // ���� ��� ��
    private int bestkillcount; // ���� �� �ְ� ��� �� ǥ��, AddKillCount()�θ� ���� ����
    public bool isGameover { get; private set; } // ���� ���� ����, EndGame()�θ� �� ���� ����

    public Gun Shotgun1;
    public Gun1 Shotgun2;
    public GameObject Shotgun1buttonblack;
    public GameObject Shotgun2buttonblack;
    public GameObject Shotgun1fire;
    public GameObject Shotgun2fire;
    public GameObject Shotgun1reload;
    public GameObject Shotgun2reload;

    // Start is called before the first frame update

    private void Awake()
    {
        // PlayerPrefs.SetInt("BestScore", 0);
        // PlayerPrefs.SetInt("BestKillCount", 0);

        bestscore = PlayerPrefs.GetInt("BestScore");
        bestkillcount = PlayerPrefs.GetInt("BestKillCount");
    }

    void Start()
    {
        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerHealth>().ActionDeath += EndGame;
        UIManager.instance.UpdateScoreText(score);
        UIManager.instance.UpdateBestScoreText(bestscore);
        UIManager.instance.UpdateKillCount(killcount);
        UIManager.instance.UPdateBestKillCount(bestkillcount);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameover)
        {
            if(score > PlayerPrefs.GetInt("BestScore"))
            {
                bestscore = score;
                PlayerPrefs.SetInt("BestScore", bestscore);
                UIManager.instance.UpdateBestScoreText(bestscore);
            }

            if(killcount > PlayerPrefs.GetInt("BestKillCount"))
            {
                bestkillcount = killcount;
                PlayerPrefs.SetInt("BestKillCount", bestkillcount);
                UIManager.instance.UPdateBestKillCount(bestkillcount);
            }
        }
    }

    public void UseShotgun1()
    {
        Shotgun1.gameObject.SetActive(true);
        Shotgun2.gameObject.SetActive(false);
        Shotgun1fire.SetActive(true);
        Shotgun2fire.SetActive(false);
        Shotgun1reload.SetActive(true);
        Shotgun2reload.SetActive(false);
        Shotgun1buttonblack.SetActive(false);
        Shotgun2buttonblack.SetActive(true);
    }
    public void UseShotgun2()
    {
        Shotgun1.gameObject.SetActive(false);
        Shotgun2.gameObject.SetActive(true);
        Shotgun1fire.SetActive(false);
        Shotgun2fire.SetActive(true);
        Shotgun1reload.SetActive(false);
        Shotgun2reload.SetActive(true);
        Shotgun1buttonblack.SetActive(true);
        Shotgun2buttonblack.SetActive(false);
    }

    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateScoreText(score);
        }
    }

    public void AddKillCount(int newCount)
    {
        // ���� ������ �ƴ� ���¿����� ��� �� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            killcount += newCount;
            // ���� UI �ؽ�Ʈ ����
            UIManager.instance.UpdateKillCount(killcount);
        }
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        UIManager.instance.SetActiveGameoverUI(true);
    }
}
