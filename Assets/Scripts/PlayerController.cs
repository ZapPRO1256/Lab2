using UnityEngine;

/// <summary>
/// Player ship: movement (WASD / Arrows), shooting (Space / Left Click), and health.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] float fireOffsetY = 0.6f;
    [SerializeField] int maxHealth = 3;
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] GameObject healthFillPrefab;

    float _nextFireTime;
    int _health;
    Camera _cam;
    float _minX, _maxX, _minY, _maxY;
    bool _useNewInput;
    Transform _healthBarFill;

    public void Start()
    {
        _health = maxHealth;
        CreateHealthBar();
        UpdateHealthBar();
        _cam = Camera.main;
        if (_cam != null && _cam.orthographic)
        {
            float h = _cam.orthographicSize;
            float w = h * _cam.aspect;
            _minX = -w + 0.5f;
            _maxX = w - 0.5f;
            _minY = -h + 0.5f;
            _maxY = h - 0.5f;
        }
        else
        {
            _minX = -8f; _maxX = 8f;
            _minY = -4f; _maxY = 4f;
        }

#if ENABLE_INPUT_SYSTEM
        _useNewInput = UnityEngine.InputSystem.InputSystem.settings != null;
#else
        _useNewInput = false;
#endif
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        float h = GetMoveInput().x;
        float v = GetMoveInput().y;
        Vector3 move = new Vector3(h, v, 0f) * (moveSpeed * Time.deltaTime);
        transform.position += move;

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, _minX, _maxX);
        p.y = Mathf.Clamp(p.y, _minY, _maxY);
        transform.position = p;

        if (GetFireInput() && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            AudioManager.Instance?.PlayPlayerShoot();
            Fire();
        }
    }

    Vector2 GetMoveInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (_useNewInput)
        {
            var k = UnityEngine.InputSystem.Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = (k.dKey.isPressed ? 1f : 0f) - (k.aKey.isPressed ? 1f : 0f);
            float y = (k.wKey.isPressed ? 1f : 0f) - (k.sKey.isPressed ? 1f : 0f);
            return new Vector2(x, y);
        }
#endif
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    bool GetFireInput()
    {
#if ENABLE_INPUT_SYSTEM
        if (_useNewInput)
        {
            var k = UnityEngine.InputSystem.Keyboard.current;
            var m = UnityEngine.InputSystem.Mouse.current;
            if (k != null && k.spaceKey.isPressed) return true;
            if (m != null && m.leftButton.isPressed) return true;
            return false;
        }
#endif
        return Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
    }

    void Fire()
    {
        GameObject bullet = ProjectilePool.Instance != null
            ? ProjectilePool.Instance.GetPlayerBullet()
            : null;
        if (bullet == null)
            return;
        bullet.transform.position = transform.position + Vector3.up * fireOffsetY;
        bullet.transform.rotation = Quaternion.identity;
        var proj = bullet.GetComponent<Projectile>();
        if (proj != null)
            proj.SetDirection(Vector2.up);
        bullet.SetActive(true);
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;
        _health -= amount;
        UpdateHealthBar();
        if (_health <= 0)
        {
            GameManager.Instance?.OnPlayerDeath();
            Destroy(gameObject);
        }
    }
    void CreateHealthBar()
    {
        if (healthBarPrefab == null || healthFillPrefab == null)
        {
            Debug.LogWarning("Health bar prefabs not assigned!");
            return;
        }

        GameObject barRoot = new GameObject("HealthBar");
        barRoot.transform.SetParent(transform);
        barRoot.transform.localPosition = new Vector3(0, -2f, 0);

        // Background
        GameObject bg = Instantiate(healthBarPrefab, barRoot.transform);
        bg.transform.localPosition = Vector3.zero;

        // Fill
        GameObject fill = Instantiate(healthFillPrefab, barRoot.transform);
        fill.transform.localPosition = new Vector3(-0.65f, 0f, 0f);

        _healthBarFill = fill.transform;
    }

    void UpdateHealthBar()
    {
        if (_healthBarFill == null) return;

        float percent = Mathf.Clamp01((float)_health / maxHealth)*8;
        _healthBarFill.localScale = new Vector3(percent, 3f, 0f);

        var sr = _healthBarFill.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.Lerp(Color.red, Color.green, percent);
    }
}
