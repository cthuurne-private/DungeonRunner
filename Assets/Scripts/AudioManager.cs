using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource levelMusic, gameOverMusic, winMusic;

    public AudioSource[] sfx;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void PlayGameOver()
    {
        levelMusic.Stop();

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();

        winMusic.Play();
    }

    public void PlaySFX(int soundToPlay)
    {
        sfx[soundToPlay].Stop();
        sfx[soundToPlay].Play();
    }
}
