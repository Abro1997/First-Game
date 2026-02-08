using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private PlayerHealth playerHealth;

    public enum MusicState
    {
        Gameplay,
        Shop,
        BossFight,
        GameOver
    }

    private MusicState currentMusicState = (MusicState)(-1);

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip shopMusic;
    [SerializeField] private AudioClip bossFightMusic;
    [SerializeField] private AudioClip gameOverMusic;

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
        PlayMusic(MusicState.Gameplay);
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

    public void PlayMusic(MusicState state)
    {
        if (currentMusicState == state)
            return;

        AudioClip newClip = GetClipFromState(state);
        if (newClip == null)
            return;

        currentMusicState = state;

        musicSource.clip = newClip;

        musicSource.loop = state != MusicState.GameOver;

        musicSource.Play();
    }

    public void ForcePlayMusic(MusicState state)
    {
        currentMusicState = (MusicState)(-1);
        PlayMusic(state);
    }

    private AudioClip GetClipFromState(MusicState state)
    {
        switch (state)
        {
            case MusicState.Gameplay: return backgroundMusic;
            case MusicState.Shop: return shopMusic;
            case MusicState.BossFight: return bossFightMusic;
            case MusicState.GameOver: return gameOverMusic;
            default: return null;
        }
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    private void OnEnemySpawned(object sender, EventArgs e)
    {
        PlayRandomSFX(enemySpawnClips);
    }

    private void OnPlayerHealthChanged(object sender, PlayerHealth.OnHealthChangedEventArgs e)
    {
        PlayRandomSFX(damageClips);
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
