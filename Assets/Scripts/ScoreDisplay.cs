using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private static ScoreDisplay instance; // Singleton instance
    public Text skorText;
    public static ScoreDisplay Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreDisplay>();
            }
            return instance;
        }
    }

    void Start()
    {
        // Cari objek QuizManager di seluruh scene, termasuk yang tidak aktif
        QuizManager quizManager = GameObject.FindObjectOfType<QuizManager>();

        // Cek apakah QuizManager ditemukan
        if (quizManager != null)
        {
            // Mendapatkan skor awal dari QuizManager
            int skorAwal = quizManager.DapatkanSkor();

            // Update tampilan skor menggunakan skor awal
            UpdateScoreDisplay(skorAwal);

            // Periksa dan aktifkan Stage2 berdasarkan skor awal
            quizManager.CheckAndActivateStage2(skorAwal);
        }
        else
        {
            Debug.LogError("QuizManager tidak ditemukan. Mungkin belum aktif atau belum diinisialisasi.");
        }
    }

    // Fungsi untuk mengupdate tampilan skor
    public void UpdateScoreDisplay(int skor)
    {
        // Perbarui teks pada objek teks (skorText) dengan skor yang dimiliki
        if (skorText != null)
        {
            // Batasi tampilan skor maksimal menjadi 5
            int tampilanSkor = Mathf.Min(skor, 5);
            skorText.text = "Misi : " + tampilanSkor + "/5";
        }
        else
        {
            Debug.LogError("Text pada skorText tidak terhubung.");
        }
    }
}
