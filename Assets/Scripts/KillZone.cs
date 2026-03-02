using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ‗ךשמ צו גמנמד
        var enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (EnemyPool.Instance != null)
                EnemyPool.Instance.ReturnEnemy(enemy.gameObject);
            else
                Destroy(enemy.gameObject);

            return;
        }

        // ‗ךשמ צו ךףכÿ
        var projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            other.gameObject.SetActive(false);
            return;
        }

        // Âסו ³םרו
        other.gameObject.SetActive(false);
    }
}