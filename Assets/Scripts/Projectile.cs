using UnityEngine;

/// <summary>
/// Projectile (bullet) movement and collision. Used for both player and enemy shots.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    [SerializeField] bool isPlayerProjectile = true;
    [SerializeField] int damage = 1;

    Vector2 _direction = Vector2.up;
    Rigidbody2D _rb;

    public bool IsPlayerProjectile => isPlayerProjectile;
    public int Damage => damage;

    public void SetIsPlayer(bool isPlayer)
    {
        isPlayerProjectile = isPlayer;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
            _rb = gameObject.AddComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerProjectile)
        {
            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<EnemyController>();
                if (enemy != null)
                    enemy.TakeDamage(damage);
                ReturnOrDestroy();
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerController>();
                if (player != null)
                    player.TakeDamage(damage);
                ReturnOrDestroy();
            }
        }
    }

    void OnBecameInvisible()
    {
        ReturnOrDestroy();
    }

    void ReturnOrDestroy()
    {
        if (ProjectilePool.Instance != null)
        {
            gameObject.SetActive(false);
            transform.SetParent(ProjectilePool.Instance.transform);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
