using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Música")]
    public AudioClip mapaMusic;
    public AudioClip interiorMusic;
    public AudioClip battleMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ChangeMusicByScene(SceneManager.GetActiveScene().path);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeMusicByScene(scene.path);
    }

    void ChangeMusicByScene(string scenePath)
    {
        AudioClip newClip = null;

        if (scenePath.Contains("Juego"))
        {
            newClip = battleMusic;
        }
        else if (scenePath.Contains("Interiores"))
        {
            newClip = interiorMusic;
        }
        else
        {
            newClip = mapaMusic;
        }

        if (audioSource.clip != newClip)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}