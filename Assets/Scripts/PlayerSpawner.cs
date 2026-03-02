using UnityEngine;

/// <summary>
/// Spawns player on game start and after restart.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Vector3 spawnPosition = new Vector3(0, -3.5f, 0);

    GameObject _currentPlayer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (_currentPlayer != null)
            Destroy(_currentPlayer);

        _currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}