using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �ڵ�
using UnityEngine.UI; // UI ���� �ڵ�

// �ʿ��� UI�� ��� �����ϰ� ������ �� �ֵ��� ����ϴ� UI �Ŵ���
public class UIManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
    public static UIManager instance // m_instance�� ����Ű�� ������ ����->Ÿ Ŭ���������� instance�� ����ϸ� ��
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>(); // UIManager�� ���� ���ӿ�����Ʈ�� m_instance�� ����Ŵ
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // �̱����� �Ҵ�� ����, ���� ���ӿ�����Ʈ(��ũ��Ʈ ���� ������Ʈ)�� �� ������ �Ҵ� ��

    public TextMeshProUGUI[] ammoText; // ź�� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI stageText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI bestscoreText; // �ְ� ���� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI killcountText; // ����� �� �� ǥ�ÿ� �ؽ�Ʈ
    public TextMeshProUGUI bestkillcountText; // �ְ� ��� �� �� ǥ�ÿ� �ؽ�Ʈ
    public GameObject gameoverUI; // ���� ������ Ȱ��ȭ�� UI 


    // ź�� �ؽ�Ʈ ����
    public void UpdateGun1AmmoText(int magAmmo, int remainAmmo)
    {
        ammoText[0].text = magAmmo + "/" + remainAmmo;
    }

    public void UpdateGun2AmmoText(int magAmmo, int remainAmmo)
    {
        ammoText[1].text = magAmmo + "/" + remainAmmo;
    }

    // ���� �ؽ�Ʈ ����
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : " + newScore;
    }

    // �ְ� ���� �ؽ�Ʈ ����
    public void UpdateBestScoreText(int bestScore)
    {
        bestscoreText.text = "BestScore : " + bestScore;
    }

    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int stages, int count)
    {
        stageText.text = "Stage : " + stages + "\nEnemy Left : " + count;
    }

    // �� ��� �� �ؽ�Ʈ ����
    public void UpdateKillCount(int count)
    {
        killcountText.text = "KillCount: " + count;
    }
    
    // �ְ� ��� �� �ؽ�Ʈ ����
    public void UPdateBestKillCount(int bestCount)
    {
        bestkillcountText.text = "Best KillCount: " + bestCount;
    }

    // ���� ���� UI Ȱ��ȭ
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    // ���� �����
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε� ��
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