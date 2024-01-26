using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class menu_utama : MonoBehaviour
{
    public GameObject panelmenu;
    public GameObject settings;
    public GameObject loading;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        panelmenu.SetActive(true);
        settings.SetActive(false);

        if (audioManager != null)
        {
            // Tambahkan listener untuk event perubahan volume music dan sfx
            audioManager.OnMusicVolumeChanged += UpdateMusicVolumeSlider;
            audioManager.OnSFXVolumeChanged += UpdateSFXVolumeSlider;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TombolSelect(string sceneName)
    {
        // Aktifkan objek loading sebelum memuat scene
        StartCoroutine(LoadSceneWithLoading(sceneName));
    }

    IEnumerator LoadSceneWithLoading(string sceneName)
    {
        // Aktifkan objek loading
        loading.SetActive(true);

        // Tunggu sejenak untuk memberikan kesan animasi loading (opsional)
        yield return new WaitForSeconds(3.0f);

        // Mulai proses loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Tunggu hingga proses loading selesai
        while (!asyncLoad.isDone)
        {
            // Jeda sementara (bisa tambahkan animasi loading atau progres bar di sini)
            yield return null;
        }

        // Nonaktifkan objek loading setelah selesai loading
        loading.SetActive(false);

        // Pindah ke scene tujuan
        // PilihKarakterDanLanjutkan();

        // Memanggil fungsi UpdateSkor setelah memuat scene
        // UpdateSkor(0); // Ganti dengan nilai tambahan skor yang sesuai
        // PlayButtonClickSFX();
        // SetKursorBentuk();
    }

    // Fungsi untuk memperbarui slider volume music saat terjadi perubahan volume
    private void UpdateMusicVolumeSlider(float volume)
    {
        if (audioManager.musicVolumeSlider != null)
        {
            audioManager.musicVolumeSlider.value = volume;
        }
    }

    // Fungsi untuk memperbarui slider volume sfx saat terjadi perubahan volume
    private void UpdateSFXVolumeSlider(float volume)
    {
        if (audioManager.sfxVolumeSlider != null)
        {
            audioManager.sfxVolumeSlider.value = volume;
        }
    }

    // Fungsi untuk menghentikan backsound gameplay dan kembali ke backsound default lobby
    public void StopGameplayMusicAndPlayDefault()
    {
        // Hentikan backsound gameplay
        AudioManager.Instance.StopBackgroundMusic();

        // Kembali ke backsound default lobby (indeks 0)
        AudioManager.Instance.ChangeBackgroundMusicByIndex(0);

        // Mulai kembali backsound default lobby
        AudioManager.Instance.PlayBackgroundMusic();
    }

    public void TombolSettings()
    {
        panelmenu.SetActive(false);
        settings.SetActive(true);
        // audioManager.PlaySFX(audioManager.buttonClick);
    }

    public void TombolBack()
    {
        panelmenu.SetActive(true);
        settings.SetActive(false);
        // audioManager.PlaySFX(audioManager.buttonClick);
    }

    public void TombolQuit()
    {
        Application.Quit();
        // audioManager.PlaySFX(audioManager.buttonClick);
    }
}

