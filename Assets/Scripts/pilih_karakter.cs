// pilih_karakter.cs
using UnityEngine;
using UnityEngine.UI;

public class pilih_karakter : MonoBehaviour
{
    public Texture2D cursorOption1;
    public Texture2D cursorOption2;

    private int selectedCursorIndex;
    private CursorMode cursorMode = CursorMode.Auto;

    public ScrollRect scrollRect;

    public static string SelectedCursorKey = "SelectedCursor";

    void Start()
    {
        // Inisialisasi nilai awal
        UpdateCursor();
    }

    void Update()
    {
        // Perbarui logika ScrollRect jika diperlukan
    }

    void UpdateCursor()
    {
        // Mengubah kursor berdasarkan nilai selectedCursorIndex
        if (selectedCursorIndex == 0)
        {
            Cursor.SetCursor(cursorOption1, Vector2.zero, cursorMode);
        }
        else if (selectedCursorIndex == 1)
        {
            Cursor.SetCursor(cursorOption2, Vector2.zero, cursorMode);
        }
    }

    public void SetCustomCursor()
    {
        // Mengambil indeks karakter yang dipilih dari PlayerPrefs
        int selectedCursor = PlayerPrefs.GetInt(SelectedCursorKey, 0);

        // Memanggil SetCustomCursor dengan indeks karakter yang dipilih
        SetCustomCursor(selectedCursor);
    }

    public void SetCustomCursor(int index)
    {
        selectedCursorIndex = index;
        PlayerPrefs.SetInt(SelectedCursorKey, selectedCursorIndex);
        PlayerPrefs.Save();
        UpdateCursor();
    }
}
