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
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int score = 0; // 현재 게임 점수, AddScore()로만 접근 가능
    private int bestscore; // 시작 시 최고 점수 표시
    private int killcount = 0; // 현재 사살 수
    private int bestkillcount; // 시작 시 최고 사살 수 표시, AddKillCount()로만 접근 가능
    public bool isGameover { get; private set; } // 게임 오버 상태, EndGame()로만 값 변경 가능

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
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
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
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }

    public void AddKillCount(int newCount)
    {
        // 게임 오버가 아닌 상태에서만 사살 수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            killcount += newCount;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateKillCount(killcount);
        }
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }
}
