using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// In-game UI: score, lives, game over panel and restart button.
/// Assign UI elements in Inspector or leave null to use placeholders from GameBootstrap.
/// </summary>
public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [Header("Optional - assign in Inspector or leave null to use placeholders")]
    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Button restartButton;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }

    void OnRestartClicked()
    {
        GameManager.Instance?.RestartGame();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    public void ShowGameOver(bool show)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(show);
    }
}
