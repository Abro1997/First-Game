using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private Player player;
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Damage SFX")]
    [SerializeField] private AudioClip[] damageClips;

    [Header("Enemy Spawn SFX")]
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

        Enemy.OnEnemySpawn += OnEnemySpawned;

    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnHealthChanged -= OnPlayerHealthChanged;
        }

        Enemy.OnEnemySpawn -= OnEnemySpawned;
    }

    private void Start()
    {
        PlayMusic();

        player = UnityEngine.Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.OnHealthChanged += OnPlayerHealthChanged;
        }
    }

    private void OnEnemySpawned(object sender, EventArgs e)
    {
        PlayRandomSFX(enemySpawnClips);
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
    private void OnPlayerHealthChanged(object sender, Player.OnPlayerHealthChanged e)
    {
        PlayRandomSFX(damageClips);
    }
}
