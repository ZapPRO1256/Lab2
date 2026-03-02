using UnityEngine;

/// <summary>
/// Spawns enemies at random X positions above the screen at intervals.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float spawnXMin = -7f;
    [SerializeField] float spawnXMax = 7f;
    [SerializeField] float spawnY = 6f;
    [SerializeField] bool usePool = true;

    float _nextSpawnTime;
    Camera _cam;

    void Start()
    {
        _nextSpawnTime = Time.time + 1f;
        _cam = Camera.main;
        if (_cam != null && _cam.orthographic)
        {
            float h = _cam.orthographicSize;
            float w = h * _cam.aspect;
            spawnXMin = -w + 0.5f;
            spawnXMax = w - 0.5f;
            spawnY = h + 0.5f;
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        if (Time.time < _nextSpawnTime)
            return;

        _nextSpawnTime = Time.time + spawnInterval;

        GameObject enemy = usePool && EnemyPool.Instance != null
            ? EnemyPool.Instance.GetEnemy()
            : null;

        if (enemy != null)
        {
            float x = Random.Range(spawnXMin, spawnXMax);
            enemy.transform.position = new Vector3(x, spawnY, 0f);
            enemy.transform.rotation = Quaternion.identity;
            enemy.transform.SetParent(null);
            enemy.SetActive(true);
        }
    }
    void Spawn_Player()
    {

    }
}
