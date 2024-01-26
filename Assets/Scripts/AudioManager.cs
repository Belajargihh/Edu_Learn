using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; // Tambahkan referensi ke Audio Mixer

    [Header("------------------- Audio Source ------------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("------------------- Audio Clip --------------------")]
    [SerializeField] private AudioClip[] backgroundClips; // Gunakan array untuk menyimpan beberapa backsound gameplay
    [SerializeField] private AudioClip jawabanBenar;
    [SerializeField] private AudioClip jawabanSalah;
    [SerializeField] public AudioClip buttonClick;

    [Header("------------------- Sliders --------------------")]
    [SerializeField] public Slider musicVolumeSlider;
    [SerializeField] public Slider sfxVolumeSlider;

    // Tambahkan variabel static untuk menyimpan referensi objek settings
    private static GameObject settingsObject;

    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;

    // Tambahkan variabel untuk menyimpan indeks backsound gameplay saat ini
    private int currentBackgroundIndex = 0;

    // Event untuk memberi tahu perubahan volume pada music dan sfx
    public event Action<float> OnMusicVolumeChanged;
    public event Action<float> OnSFXVolumeChanged;

    private static AudioManager instance; // Singleton instance

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }

    // Tambahkan properti publik untuk mendapatkan indeks backsound saat ini
    public int CurrentBackgroundIndex
    {
        get { return currentBackgroundIndex; }
    }

    // Tambahkan metode publik untuk mengatur backsound berdasarkan indeks
    public void SetBackgroundMusic(int index)
    {
        if (musicSource != null && backgroundClips.Length > index && backgroundClips[index] != null)
        {
            musicSource.clip = backgroundClips[index];
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Music Source or Background Audio Clip is not set in AudioManager.");
        }
        currentBackgroundIndex = index;
    }

    private void Awake()
    {
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(musicVolumeSlider.gameObject);
            DontDestroyOnLoad(sfxVolumeSlider.gameObject);

            // Periksa apakah settingsObject sudah diinisialisasi
            if (settingsObject == null)
            {
                settingsObject = GameObject.Find("Settings"); // Ganti "SettingsObject" dengan nama objek settings di scene pertama
            }
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();

        if (musicVolumeSlider != null && sfxVolumeSlider != null)
        {
            musicSource.clip = backgroundClips[0];
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Music Source or Background Audio Clip is not set in AudioManager.");
        }

        if (musicVolumeSlider != null && sfxVolumeSlider != null && settingsObject != null)
        {
            // Set nilai awal dari slider sesuai dengan nilai default volume
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;

            // Tambahkan listener untuk slider agar dapat memperbarui volume saat digeser
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolumeFromSlider);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolumeFromSlider);
        }
        else
        {
            Debug.LogError("Music Volume Slider or SFX Volume Slider or Settings Object is not set in AudioManager.");
        }
        // Inisialisasi nilai volume Audio Mixer
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    // Fungsi untuk mengubah backsound saat memasuki tahap berikutnya
    public void ChangeBackgroundMusic()
    {
        // Hentikan backsound lobby sebelum memainkan backsound gameplay
        StopBackgroundMusic();

        // Tambahkan logika untuk mengganti backsound (misalnya, jika mencapai akhir level, kembali ke backsound pertama)
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgroundClips.Length;
        SetBackgroundMusic(currentBackgroundIndex);
    }

    // Fungsi untuk mengubah backsound berdasarkan indeks dari list di Inspector
    // Fungsi untuk mengubah backsound berdasarkan indeks dari list di Inspector
    public void ChangeBackgroundMusicByIndex(int index)
    {
        // Hentikan backsound lobby sebelum memainkan backsound gameplay
        StopBackgroundMusic();

        // Set backsound sesuai dengan indeks yang diberikan
        if (index >= 0 && index < backgroundClips.Length)
        {
            SetBackgroundMusic(index);
            currentBackgroundIndex = index;

            // Debug untuk memeriksa nilai index
            Debug.Log("Changed background music to index: " + index);
        }
        else
        {
            Debug.LogError("Invalid index for background music.");
        }
    }

    // Fungsi untuk memperbarui volume musik saat slider digeser
    public void SetMusicVolumeFromSlider(float volume)
    {
        SetMusicVolume(volume);
    }

    // Fungsi untuk memperbarui volume sfx saat slider digeser
    public void SetSFXVolumeFromSlider(float volume)
    {
        SetSFXVolume(volume);
    }

    // Fungsi untuk mengatur volume musik
    // Fungsi untuk mengatur volume musik dari slider
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (audioMixer != null)
        {
            float logVolume = Mathf.Log10(volume) * 20;
            audioMixer.SetFloat("MusicVolume", logVolume);
            // Memanggil event untuk memberi tahu perubahan volume
            OnMusicVolumeChanged?.Invoke(volume);
        }
    }

    // Fungsi untuk mengatur volume sfx dari slider
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        if (audioMixer != null)
        {
            float logVolume = Mathf.Log10(volume) * 20;
            audioMixer.SetFloat("SFXVolume", logVolume);
            // Memanggil event untuk memberi tahu perubahan volume
            OnSFXVolumeChanged?.Invoke(volume);
        }
    }

    // Fungsi untuk menghentikan backsound gameplay dan kembali ke backsound default lobby
    public void StopGameplayMusicAndPlayDefault()
    {
        // Hentikan backsound gameplay
        StopBackgroundMusic();

        // Kembali ke backsound default lobby (indeks 0)
        SetBackgroundMusic(0);

        // Mulai kembali backsound default lobby
        PlayBackgroundMusic();
    }

    public void PlayCorrectAnswerSound()
    {
        PlaySFX(jawabanBenar);
    }

    public void PlayWrongAnswerSound()
    {
        PlaySFX(jawabanSalah);
    }


    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop(); // Menghentikan sumber suara latar belakang
        }
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Play(); // Memulai musik latar belakang
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    // Fungsi untuk memainkan suara saat tombol diklik
    public void PlayButtonClickSound()
    {
        if (SFXSource != null && buttonClick != null)
        {
            SFXSource.PlayOneShot(buttonClick);
        }
    }

    public void OnButtonClick()
    {
        // Memainkan suara saat tombol diklik
        PlayButtonClickSound();
    }
}
