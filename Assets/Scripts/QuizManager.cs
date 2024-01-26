using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public Quiz currentQuiz; // Satu elemen Quiz untuk semua pertanyaan
    public InputField answerField; // Menggunakan satu InputField saja

    public GameObject Quizpanel;
    public GameObject Finish;
    public GameObject feedBenar;
    public GameObject feedSalah;

    private int score;

    private bool pertanyaanSudahDijawab = false;
    public GameObject gameplay1; // Deklarasikan variabel gameplay1
    public GameObject gameplay2; // Deklarasikan variabel gameplay2
    public GameObject gameplay3; // Deklarasikan variabel gameplay3
    public GameObject gameplay4; // Deklarasikan variabel gameplay4
    public GameObject gameplay5;

    public GameObject animasiBenar;
    public GameObject animasiSalah;
    private int currentStage = 1;
    private int completedGameplaysInCurrentStage = 0;


    private static QuizManager instance; // Singleton instance

    public static QuizManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuizManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        Finish.SetActive(false);
        score = PlayerPrefs.GetInt("Skor", 0);
        ScoreDisplay.Instance.UpdateScoreDisplay(score);

        SetPertanyaan();
    }

    private void Update()
    {
        // Check if the return key is pressed in the answer field
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }
    }


    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        Quizpanel.SetActive(false);

        // Simpan skor saat keluar game
        PlayerPrefs.SetInt("Skor", score);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs

        Finish.SetActive(true);
    }

    public void SubmitAnswer()
    {
        if (pertanyaanSudahDijawab)
        {
            return;
        }

        string jawabanPemain = answerField.text;

        if (jawabanPemain.ToLower() == currentQuiz.Jawaban.ToLower())
        {
            if (score < 5)
            {
                score += 1;
            }
            pertanyaanSudahDijawab = true;

            ScoreDisplay.Instance.UpdateScoreDisplay(score);
            AudioManager.Instance.PlayCorrectAnswerSound();

            StartCoroutine(PlayAnimation(feedBenar, 2.0f));
            PlayerPrefs.SetInt("Skor", score);
            PlayerPrefs.Save();

            StartCoroutine(DisableGameplayAfterDelay(gameplay1, 2.0f));
            StartCoroutine(DisableGameplayAfterDelay(gameplay2, 2.0f));
            StartCoroutine(DisableGameplayAfterDelay(gameplay3, 2.0f));
            StartCoroutine(DisableGameplayAfterDelay(gameplay4, 2.0f));
            StartCoroutine(DisableGameplayAfterDelay(gameplay5, 2.0f));

            completedGameplaysInCurrentStage++;

            if (completedGameplaysInCurrentStage == 5) // Jika semua gameplay di stage ini selesai
            {
                // Atur ulang skor dan lanjut ke stage berikutnya
                score = 0;
                completedGameplaysInCurrentStage = 0;
                currentStage++;

                if (currentStage > 5) // Misalnya, jika total stage adalah 3
                {
                    // Permainan selesai
                    GameOver();
                }
                else
                {
                    // Menampilkan pertanyaan pada objek gameplay baru untuk stage berikutnya
                    SetPertanyaan();
                }
            }
            else
            {
                // Menampilkan pertanyaan pada objek gameplay baru di stage yang sama
                StartCoroutine(TampilkanGameplaySelanjutnyaDenganDelay(2.0f));
            }
        }
        else
        {
            AudioManager.Instance.PlayWrongAnswerSound();
            StartCoroutine(PlayAnimation(feedSalah, 2.0f));
        }
    }


    // Coroutine untuk menampilkan animasi dengan waktu penundaan yang dapat diatur
    IEnumerator PlayAnimation(GameObject animasiObj, float delay)
    {
        animasiObj.SetActive(true);
        yield return new WaitForSeconds(delay); // Sesuaikan waktu penundaan sesuai kebutuhan
        animasiObj.SetActive(false);
    }

    // Coroutine untuk menunda tampilan gameplay selanjutnya dengan waktu penundaan yang dapat diatur
    IEnumerator TampilkanGameplaySelanjutnyaDenganDelay(float delay)
    {
        // Tunggu beberapa saat sebelum menampilkan gameplay selanjutnya
        yield return new WaitForSeconds(delay); // Sesuaikan waktu penundaan sesuai kebutuhan

        // Menampilkan pertanyaan pada objek gameplay baru
        SetPertanyaan();

        // Set pertanyaanSudahDijawab menjadi false untuk pertanyaan baru
        pertanyaanSudahDijawab = false;

        // Menonaktifkan kembali semua gameplay setelah menampilkan pertanyaan baru
        SetGameplaysActive(true);
    }

    IEnumerator DisableGameplayAfterDelay(GameObject gameplay, float delay)
    {
        // Tunggu beberapa saat sebelum menonaktifkan gameplay
        yield return new WaitForSeconds(delay); // Sesuaikan waktu penundaan sesuai kebutuhan

        // Menonaktifkan gameplay setelah delay
        gameplay.SetActive(false);
    }

    // Fungsi untuk mengatur apakah semua gameplays aktif atau tidak
    private void SetGameplaysActive(bool active)
    {
        gameplay1.SetActive(active);
        gameplay2.SetActive(active);
        gameplay3.SetActive(active);
        gameplay4.SetActive(active);
        gameplay5.SetActive(active);
    }

    void ShowNextGameplay()
    {
        pertanyaanSudahDijawab = false; // Setel kembali ke kondisi awal

        // Di sini, Anda perlu menentukan logika untuk menampilkan pertanyaan pada objek gameplay baru.
        // Misalnya, Anda bisa mengaktifkan atau menonaktifkan objek gameplay sebelumnya dan mengaktifkan yang baru.

        // Menampilkan pertanyaan pada objek gameplay baru
        SetPertanyaan();
    }

    private void UpdateSkor(int tambahanSkor)
    {
        // Dapatkan skor saat ini dari PlayerPrefs
        int skor = PlayerPrefs.GetInt("Skor", 0);

        // Tambahkan nilai tambahan skor
        skor += tambahanSkor;

        // Simpan skor ke PlayerPrefs
        PlayerPrefs.SetInt("Skor", skor);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs

        // Periksa apakah skor mencapai batas maksimal dan aktifkan Stage2 jika iya
        CheckAndActivateStage2(skor);

        // Update tampilan skor
        ScoreDisplay.Instance?.UpdateScoreDisplay(skor);
    }

    public void CheckAndActivateStage2(int skor)
    {
        if (skor >= 5)
        {
            // Panggil fungsi AktifkanStage2 pada ButtonAction
            FindObjectOfType<ButtonAction>()?.AktifkanStage2();
        }
    }

    void AturSkorDanSimpan()
    {
        // Menyimpan nilai skor ke PlayerPrefs saat permainan berakhir
        PlayerPrefs.SetInt("Skor", score);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs
        Debug.Log("Skor Akhir: " + score + "/5");
        // Tambahan logika lain yang mungkin diperlukan terkait dengan skor
    }

    public void SetPertanyaan()
    {
        // Menampilkan pertanyaan pada suatu objek teks atau sesuai kebutuhan
        Debug.Log("Pertanyaan: " + currentQuiz.Pertanyaan);
    }

    // Metode untuk mendapatkan skor
    public int DapatkanSkor()
    {
        return score;
    }
}
