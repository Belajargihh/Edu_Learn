using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public GameObject[] gameplays;

    public GameObject alert;
    public string karakterDipilih;
    public GameObject settings;
    public GameObject gameplay_1;
    public GameObject gameplay_2;
    public GameObject gameplay_3;
    public GameObject gameplay_4;
    public GameObject gameplay_5;
    public GameObject loading;
    public Texture2D yourCustomCursorTexture1;
    public Texture2D yourCustomCursorTexture2;
    public GameObject karakterCursorContainer;
    private AudioManager audioManager;
    public ScrollRect karakterScrollRect;
    public Slider cursorSizeSlider;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    public GameObject animasiBenar;
    public GameObject animasiSalah;
    private bool kustomKursor = false;
    private int skor = 0;
    private string skorKey = "Skor";
    private bool skorTersimpan = false;
    private static int defaultBackgroundIndex = 0;
    private pilih_karakter karakterScript;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        karakterScript = FindObjectOfType<pilih_karakter>();
    }

    private void Start()
    {
        settings.SetActive(false);
        skor = PlayerPrefs.GetInt(skorKey, 0);
        defaultBackgroundIndex = AudioManager.Instance.CurrentBackgroundIndex;

        if (audioManager != null)
        {
            audioManager.OnMusicVolumeChanged += UpdateMusicVolumeSlider;
            audioManager.OnSFXVolumeChanged += UpdateSFXVolumeSlider;
        }

        UpdateSkorDisplay();
        SetCustomCursor();
    }

    public void SetCustomCursor()
    {
        // Mendapatkan objek pilih_karakter
        pilih_karakter karakterScript = FindObjectOfType<pilih_karakter>();

        if (karakterScript != null)
        {
            // Memanggil SetCustomCursor tanpa parameter
            karakterScript.SetCustomCursor();
        }
    }

    private void UpdateMusicVolumeSlider(float volume)
    {
        if (audioManager.musicVolumeSlider != null)
        {
            audioManager.musicVolumeSlider.value = volume;
        }
    }

    private void UpdateSFXVolumeSlider(float volume)
    {
        if (audioManager.sfxVolumeSlider != null)
        {
            audioManager.sfxVolumeSlider.value = volume;
        }
    }

    private void UbahBentukKursor(bool kustom)
    {
        kustomKursor = kustom;
        SetKursorBentuk();
        PlayButtonClickSFX();

        // Panggil metode SetCustomCursor dari pilih_karakter
        if (karakterScript != null)
        {
            int kursorIndex = kustomKursor ? 0 : 1; // Sesuaikan dengan indeks karakter yang ingin digunakan
            karakterScript.SetCustomCursor(kursorIndex);
        }
    }

    private void SetKursorBentuk()
    {
        if (kustomKursor)
        {
            if (yourCustomCursorTexture1 != null)
            {
                Cursor.SetCursor(yourCustomCursorTexture1, hotSpot, cursorMode);
            }
            else
            {
                Debug.LogError("Texture kursor 1 belum di-set.");
            }
        }
        else
        {
            if (yourCustomCursorTexture2 != null)
            {
                Cursor.SetCursor(yourCustomCursorTexture2, hotSpot, cursorMode);
            }
            else
            {
                Debug.LogError("Texture kursor 2 belum di-set.");
            }
        }
    }

    public void StopGameplayMusicAndPlayDefault()
    {
        // Hentikan backsound gameplay
        AudioManager.Instance.StopBackgroundMusic();

        // Kembali ke backsound default lobby (indeks 0)
        AudioManager.Instance.SetBackgroundMusic(defaultBackgroundIndex);

        // Mulai kembali backsound default lobby
        AudioManager.Instance.PlayBackgroundMusic();
    }

    public void TombolKustomKursor()
    {
        UbahBentukKursor(true);
    }

    public void TombolDefaultKursor()
    {
        UbahBentukKursor(false);
    }

    public void PilihKarakterDanLanjutkan()
    {
        // PlayerPrefs.SetString("KarakterDipilih", karakterDipilih); // Tidak perlu lagi
        SceneManager.LoadScene("");
    }

    public void OpenGameplay(int stageIndex)
    {
        string stageSceneName = "Stage" + stageIndex;
        SetGameplayActive(false);
        SceneManager.LoadScene(stageSceneName);
        PlayButtonClickSFX();
    }

    public void AktifkanStage2()
    {
        // Temukan objek Stage2 pada scene PilihanTempat
        GameObject stage2Obj = GameObject.Find("Stage2");

        // Memeriksa apakah objek ditemukan
        if (stage2Obj != null)
        {
            // Mendapatkan komponen Button pada objek Stage2
            Button tombolStage2 = stage2Obj.GetComponent<Button>();

            // Memeriksa apakah komponen Button ditemukan
            if (tombolStage2 != null)
            {
                // Mengaktifkan interaksi pada tombol Stage2
                tombolStage2.interactable = true;
            }
            else
            {
                Debug.LogError("Komponen Button tidak ditemukan pada objek Stage2.");
            }
        }
        else
        {
            Debug.LogError("Objek Stage2 tidak ditemukan pada scene PilihanTempat.");
        }
    }

    public void OpenURL()
    {
        Application.OpenURL("https://www.canva.com/design/DAF6WVQ8AiA/S-HTxDzWT0exqKjuWwwxrw/edit");
    }

    public void TombolSettings()
    {
        SetGameplayActive(false);
        settings.SetActive(!settings.activeSelf);
        PlayButtonClickSFX();
    }

    public void gameplay1()
    {
        SetGameplayActive(false);
        gameplay_1.SetActive(!gameplay_1.activeSelf);
        PlayButtonClickSFX();
    }

    public void gameplay2()
    {
        SetGameplayActive(false);
        gameplay_2.SetActive(!gameplay_2.activeSelf);
        PlayButtonClickSFX();
    }

    public void gameplay3()
    {
        SetGameplayActive(false);
        gameplay_3.SetActive(!gameplay_3.activeSelf);
        PlayButtonClickSFX();
    }

    public void gameplay4()
    {
        SetGameplayActive(false);
        gameplay_4.SetActive(!gameplay_4.activeSelf);
        PlayButtonClickSFX();
    }

    public void gameplay5()
    {
        SetGameplayActive(false);
        gameplay_5.SetActive(!gameplay_5.activeSelf);
        PlayButtonClickSFX();
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
        PlayButtonClickSFX();
    }

    public void TombolKembali2(string sceneName)
    {
        // Ubah backsound saat kembali ke menu utama
        if (sceneName == "MenuUtama" || sceneName == "PilihanTempat")
        {
            StartCoroutine(LoadSceneWithLoading2(sceneName, AudioManager.Instance.StopGameplayMusicAndPlayDefault));
        }
        else
        {
            StartCoroutine(LoadSceneWithLoading2(sceneName));
        }
    }

    IEnumerator LoadSceneWithLoading2(string sceneName, Action callback = null)
    {
        // Aktifkan objek loading
        loading.SetActive(true);

        // Tunggu sejenak untuk memberikan kesan animasi loading (opsional)
        yield return new WaitForSeconds(3.0f);

        // Jalankan callback (jika ada)
        callback?.Invoke();

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
        if (sceneName == "MenuUtama" || sceneName == "PilihanTempat")
        {
            PilihKarakterDanLanjutkan();
            UpdateSkor(0); // Ganti dengan nilai tambahan skor yang sesuai
            PlayButtonClickSFX();

            // Panggil metode SetCustomCursor dari pilih_karakter
            if (karakterScript != null)
            {
                karakterScript.SetCustomCursor();
            }
        }
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

        // Ubah backsound saat memasuki tahap atau stage
        if (sceneName == "Stage1" || sceneName == "Stage2" || sceneName == "Stage3")
        {
            // Simpan indeks backsound saat memasuki stage
            defaultBackgroundIndex = AudioManager.Instance.CurrentBackgroundIndex;
            int stageIndex = int.Parse(sceneName.Substring(sceneName.Length - 1)); // Mendapatkan indeks stage dari nama scene
            AudioManager.Instance.ChangeBackgroundMusicByIndex(stageIndex);
        }
        else if (sceneName == "PilihTempat")
        {
            AudioManager.Instance.StopGameplayMusicAndPlayDefault();
        }

        // Mulai proses loading setelah pengaturan backsound
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
        if (sceneName == "MenuUtama" || sceneName == "PilihanTempat")
        {
            PilihKarakterDanLanjutkan();
            UpdateSkor(0); // Ganti dengan nilai tambahan skor yang sesuai
            PlayButtonClickSFX();
            SetKursorBentuk();
        }
    }

    public void TombolHome(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        PlayButtonClickSFX();
    }

    public void TombolKembali(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        PlayButtonClickSFX();
    }

    public void ToggleAlert()
    {
        SetGameplayActive(false);
        alert.SetActive(!alert.activeSelf);

        PlayButtonClickSFX();
    }

    public void CloseAlert()
    {
        alert.SetActive(false);
        PlayButtonClickSFX();
    }

    public void GamePlay(string targetObjectName)
    {
        // Menonaktifkan semua gameplays
        SetGameplaysActive(false);

        // Mengaktifkan objek tujuan
        GameObject targetObject = GameObject.Find(targetObjectName);

        if (targetObject != null)
        {
            targetObject.SetActive(true);

            // Memanggil fungsi UpdateSkor saat berpindah game objek
            UpdateSkor(0); // Ganti dengan nilai tambahan skor yang sesuai
        }
        else
        {
            Debug.LogError("Objek tujuan gameplay tidak ditemukan.");
        }

        // Memainkan efek suara tombol
        PlayButtonClickSFX();

        // Menonaktifkan gameplays setelah jawaban terjawab dengan benar
        StartCoroutine(DisableGameplaysAfterDelay(2.0f)); // Ganti 2.0f dengan nilai delay yang diinginkan
    }

    IEnumerator DisableGameplaysAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Menonaktifkan kembali semua gameplays
        SetGameplaysActive(false);
    }

    // Fungsi untuk mengatur apakah semua gameplays aktif atau tidak
    private void SetGameplaysActive(bool active)
    {
        foreach (GameObject gameplay in gameplays)
        {
            gameplay.SetActive(active);
        }
    }

    // Fungsi untuk memperbarui skor
    private void UpdateSkor(int tambahanSkor)
    {
        // Dapatkan skor saat ini dari PlayerPrefs
        int skor = PlayerPrefs.GetInt("Skor", 0);

        // Tambahkan nilai tambahan skor
        skor += tambahanSkor;

        // Simpan skor ke PlayerPrefs
        PlayerPrefs.SetInt("Skor", skor);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs

        // Update tampilan skor
        UpdateSkorDisplay();
    }

    private void UpdateSkorDisplay()
    {
        // Memanggil fungsi UpdateScoreDisplay dari ScoreDisplay
        ScoreDisplay.Instance.UpdateScoreDisplay(PlayerPrefs.GetInt("Skor", 0));
    }

    public void ResetSkor()
    {
        skor = 0;
        skorTersimpan = false;

        // Hapus nilai skor dari PlayerPrefs
        PlayerPrefs.DeleteKey(skorKey);
        PlayerPrefs.Save();

        // Update tampilan skor
        UpdateSkorDisplay();
    }

    private void SetGameplayActive(bool active)
    {
        foreach (GameObject gameplay in gameplays)
        {
            gameplay.SetActive(active);
        }
    }

    private void PlayButtonClickSFX()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
    }
}
