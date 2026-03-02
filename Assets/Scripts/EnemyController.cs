using UnityEngine;

/// <summary>
/// Enemy ship: moves down, optionally shoots, gives score when destroyed.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] bool canShoot = false;
    [SerializeField] float shootInterval = 2f;
    [SerializeField] int health = 1;
    [SerializeField] int scoreValue = 100;

    float _nextShootTime;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime));

        if (canShoot && Time.time >= _nextShootTime)
        {
            _nextShootTime = Time.time + shootInterval;
            Fire();
        }
    }

    void Fire()
    {
        GameObject bullet = ProjectilePool.Instance != null
            ? ProjectilePool.Instance.GetEnemyBullet()
            : null;
        if (bullet == null) return;
        bullet.transform.position = transform.position + Vector3.down * 0.4f;
        bullet.transform.rotation = Quaternion.identity;
        var proj = bullet.GetComponent<Projectile>();
        if (proj != null)
            proj.SetDirection(Vector2.down);
        bullet.SetActive(true);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameManager.Instance?.AddScore(scoreValue);
            if (EnemyPool.Instance != null)
                EnemyPool.Instance.ReturnEnemy(gameObject);
            else
                Destroy(gameObject);
            AudioManager.Instance?.PlayEnemyDeath();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
                player.TakeDamage(1);
            if (EnemyPool.Instance != null)
                EnemyPool.Instance.ReturnEnemy(gameObject);
            else
                Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        if (transform.position.y < -10f)
        {
            if (EnemyPool.Instance != null)
                EnemyPool.Instance.ReturnEnemy(gameObject);
            else
                Destroy(gameObject);
        }
    }
}
