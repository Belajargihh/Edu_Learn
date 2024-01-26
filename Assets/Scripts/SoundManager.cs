using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] backgroundMusics; // Array musik latar belakang
    public AudioSource buttonSound;

    [SerializeField] private AudioSource SFXSource;

    private static SoundManager instance;

    public float volume = 1;
    public Slider sliderGame;
    public Slider sliderButton;

    private const string keyVolumeGame = "VOLUME_GAME";
    private const string keyVolumeButton = "VOLUME_BUTTON";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        float savedVolumeGame = PlayerPrefs.GetFloat(keyVolumeGame, 1f);
        float savedVolumeButton = PlayerPrefs.GetFloat(keyVolumeButton, 1f);

        SetGameVolume(savedVolumeGame);
        SetButtonVolume(savedVolumeButton);

        sliderGame.value = savedVolumeGame;
        sliderButton.value = savedVolumeButton;

        // Mulai memutar musik latar belakang pertama saat game dimulai
        PlayBackgroundMusic(0);
    }

    public void SetGameVolume(float sliderValue)
    {
        volume = sliderValue;
        PlayerPrefs.SetFloat(keyVolumeGame, sliderValue);

        // Atur volume semua musik latar belakang
        foreach (var bgMusic in backgroundMusics)
        {
            bgMusic.volume = sliderValue;
        }
    }

    public void SetButtonVolume(float sliderValue)
    {
        buttonSound.volume = sliderValue;
        PlayerPrefs.SetFloat(keyVolumeButton, sliderValue);
    }

    public void PlayButtonClickSound()
    {
        SFXSource.PlayOneShot(buttonSound.clip);
    }

    public void PlayBackgroundMusic(int index)
    {
        // Memastikan indeks tidak melebihi panjang array
        if (index >= 0 && index < backgroundMusics.Length)
        {
            // Matikan semua musik latar belakang yang sedang bermain
            foreach (var bgMusic in backgroundMusics)
            {
                bgMusic.Stop();
            }

            // Mulai memutar musik latar belakang sesuai indeks
            backgroundMusics[index].Play();
        }
    }
}
