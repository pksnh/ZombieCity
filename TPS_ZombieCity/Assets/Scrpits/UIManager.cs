using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리자 관련 코드
using UnityEngine.UI; // UI 관련 코드

// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance // m_instance를 가리키는 참조형 변수->타 클래스에서는 instance만 사용하면 됨
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>(); // UIManager를 가진 게임오브젝트를 m_instance가 가리킴
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수, 실제 게임오브젝트(스크립트 담은 오브젝트)는 이 변수에 할당 됨

    public TextMeshProUGUI[] ammoText; // 탄약 표시용 텍스트
    public TextMeshProUGUI scoreText; // 점수 표시용 텍스트
    public TextMeshProUGUI stageText; // 적 웨이브 표시용 텍스트
    public TextMeshProUGUI bestscoreText; // 최고 점수 표시용 텍스트
    public TextMeshProUGUI killcountText; // 사살한 적 수 표시용 텍스트
    public TextMeshProUGUI bestkillcountText; // 최고 사살 적 수 표시용 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화할 UI 


    // 탄약 텍스트 갱신
    public void UpdateGun1AmmoText(int magAmmo, int remainAmmo)
    {
        ammoText[0].text = magAmmo + "/" + remainAmmo;
    }

    public void UpdateGun2AmmoText(int magAmmo, int remainAmmo)
    {
        ammoText[1].text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // 최고 점수 텍스트 갱신
    public void UpdateBestScoreText(int bestScore)
    {
        bestscoreText.text = "BestScore : " + bestScore;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int stages, int count)
    {
        stageText.text = "Stage : " + stages + "\nEnemy Left : " + count;
    }

    // 적 사살 수 텍스트 갱신
    public void UpdateKillCount(int count)
    {
        killcountText.text = "KillCount: " + count;
    }
    
    // 최고 사살 수 텍스트 갱신
    public void UPdateBestKillCount(int bestCount)
    {
        bestkillcountText.text = "Best KillCount: " + bestCount;
    }

    // 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드 함
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}