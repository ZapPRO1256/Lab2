using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for enemies to avoid instantiate/destroy spikes.
/// </summary>
public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int poolSize = 20;

    readonly List<GameObject> _enemies = new List<GameObject>();
    Transform _poolRoot;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _poolRoot = transform;

        if (enemyPrefab != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var e = Instantiate(enemyPrefab, _poolRoot);
                e.SetActive(false);
                _enemies.Add(e);
            }
        }
    }

    /// <summary>Call from GameBootstrap when creating game without prefab assets.</summary>
    public void Init(GameObject enemyTemplate)
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
        _poolRoot = transform;
        enemyPrefab = enemyTemplate;
        for (int i = 0; i < poolSize; i++)
        {
            var e = Instantiate(enemyPrefab, _poolRoot);
            e.SetActive(false);
            _enemies.Add(e);
        }
    }

    public GameObject GetEnemy()
    {
        foreach (var e in _enemies)
        {
            if (!e.activeSelf)
                return e;
        }
        if (enemyPrefab != null)
        {
            var e = Instantiate(enemyPrefab, _poolRoot);
            _enemies.Add(e);
            return e;
        }
        return null;
    }


    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemy.transform.SetParent(_poolRoot);
    }
}
