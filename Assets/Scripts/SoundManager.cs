using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private PlayerHealth playerHealth;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip[] enemySpawnClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Enemy.OnEnemySpawn += OnEnemySpawned;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Enemy.OnEnemySpawn -= OnEnemySpawned;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusic();
        RebindPlayer();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindPlayer();
    }

    private void RebindPlayer()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= OnPlayerHealthChanged;

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.OnHealthChanged += OnPlayerHealthChanged;
    }

    private void OnEnemySpawned(object sender, EventArgs e)
    {
        PlayRandomSFX(enemySpawnClips);
    }

    private void OnPlayerHealthChanged(
        object sender,
        PlayerHealth.OnHealthChangedEventArgs e
    )
    {
        PlayRandomSFX(damageClips);
    }

    private void PlayMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    private void PlayRandomSFX(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        int index = UnityEngine.Random.Range(0, clips.Length);
        sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        sfxSource.PlayOneShot(clips[index]);
        sfxSource.pitch = 1f;
    }
}
