using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object pool for player and enemy projectiles to avoid instantiate/destroy spikes.
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }

    [SerializeField] GameObject playerBulletPrefab;
    [SerializeField] int playerPoolSize = 30;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] int enemyPoolSize = 20;

    readonly List<GameObject> _playerBullets = new List<GameObject>();
    readonly List<GameObject> _enemyBullets = new List<GameObject>();
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

        if (playerBulletPrefab != null)
            PrewarmPlayer(playerPoolSize);
        if (enemyBulletPrefab != null)
            PrewarmEnemy(enemyPoolSize);
    }

    /// <summary>Call from GameBootstrap when creating game without prefab assets.</summary>
    public void Init(GameObject playerBullet, GameObject enemyBullet)
    {
        if (Instance != null && Instance != this) return;
        Instance = this;
        _poolRoot = transform;
        playerBulletPrefab = playerBullet;
        enemyBulletPrefab = enemyBullet;
        PrewarmPlayer(playerPoolSize);
        PrewarmEnemy(enemyPoolSize);
    }

    void PrewarmPlayer(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var b = Instantiate(playerBulletPrefab, _poolRoot);
            b.SetActive(false);
            _playerBullets.Add(b);
        }
    }

    void PrewarmEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var b = Instantiate(enemyBulletPrefab, _poolRoot);
            b.SetActive(false);
            _enemyBullets.Add(b);
        }
    }

    public GameObject GetPlayerBullet()
    {
        foreach (var b in _playerBullets)
        {
            if (!b.activeInHierarchy)
                return b;
        }
        if (playerBulletPrefab != null)
        {
            var b = Instantiate(playerBulletPrefab, _poolRoot);
            _playerBullets.Add(b);
            return b;
        }
        return null;
    }

    public GameObject GetEnemyBullet()
    {
        foreach (var b in _enemyBullets)
        {
            if (!b.activeInHierarchy)
                return b;
        }
        if (enemyBulletPrefab != null)
        {
            var b = Instantiate(enemyBulletPrefab, _poolRoot);
            _enemyBullets.Add(b);
            return b;
        }
        return null;
    }
}
