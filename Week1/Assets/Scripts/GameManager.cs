using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// GameManager のインスタンス
    /// </summary>
    public static GameManager Instance;

    [SerializeField, Tooltip("ゲームオーバーUI")]
    private GameObject gameOverPanel;

    [SerializeField, Tooltip("落下数表示テキスト")]
    private TextMeshProUGUI fallCountText;

    /// <summary>
    /// 落下した床の数
    /// </summary>
    private int fallenCount = 0;

    /// <summary>
    /// ゲームオーバー状態
    /// </summary>
    private bool isGameOver = false;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ゲーム開始時処理
    /// </summary>
    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;

        UpdateFallCountUI();
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    /// <summary>
    /// リスタート処理
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 落下床数を加算する
    /// </summary>
    public void AddFallCount()
    {
        fallenCount++;

        UpdateFallCountUI();
    }

    /// <summary>
    /// 落下数UI更新
    /// </summary>
    private void UpdateFallCountUI()
    {
        if (fallCountText != null)
        {
            fallCountText.text = "Score : " + fallenCount;
        }
    }
}