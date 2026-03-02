using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Effects")]
    [SerializeField] AudioClip playerShoot;
    [SerializeField] AudioClip enemyDeath;
    [SerializeField] AudioClip playerDeath;
    [SerializeField] AudioClip gameOver;

    AudioSource _audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    public void PlayPlayerShoot()
    {
        PlayOneShot(playerShoot);
    }

    public void PlayEnemyDeath()
    {
        PlayOneShot(enemyDeath);
    }

    public void PlayPlayerDeath()
    {
        PlayOneShot(playerDeath);
    }

    public void PlayGameOver()
    {
        PlayOneShot(gameOver);
    }

    void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.PlayOneShot(clip);
    }
}