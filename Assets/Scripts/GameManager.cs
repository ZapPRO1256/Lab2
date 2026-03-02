using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton game manager: score, lives, game state, and restart.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int maxLives = 3;
    [SerializeField] int scorePerEnemy = 100;

    int _score;
    int _lives;
    bool _isGameOver;

    public int Score => _score;
    public int Lives => _lives;
    public bool IsGameOver => _isGameOver;
    public int ScorePerEnemy => scorePerEnemy;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _lives = maxLives;
        _score = 0;
        _isGameOver = false;
        GameUI.Instance?.UpdateScore(_score);
        GameUI.Instance?.UpdateLives(_lives);
    }

    public void AddScore(int amount)
    {
        if (_isGameOver) return;
        _score += amount;
        GameUI.Instance?.UpdateScore(_score);
    }

    public void OnPlayerDeath()
    {
        if (_isGameOver) return;
        _lives--;
        LostLives();
        AudioManager.Instance?.PlayPlayerDeath();
        if (_lives <= 0)
        {
            _isGameOver = true;
            GameUI.Instance?.ShowGameOver(true);
            AudioManager.Instance?.PlayGameOver();
        }
    }

    public void RestartGame()
    {
        _score = 0;
        _lives = maxLives;
        _isGameOver = false;

        GameUI.Instance?.ShowGameOver(false);
        GameUI.Instance?.UpdateScore(_score);
        GameUI.Instance?.UpdateLives(_lives);

        PlayerSpawner.Instance?.SpawnPlayer();

    }
    public void LostLives()
    {

        GameUI.Instance?.UpdateScore(_score);
        GameUI.Instance?.UpdateLives(_lives);
        PlayerSpawner.Instance?.SpawnPlayer();
    }
}
