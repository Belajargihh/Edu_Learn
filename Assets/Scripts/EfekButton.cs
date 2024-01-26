using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EfekButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color normalColor;
    private Color hoverColor = Color.gray; // Ganti dengan warna yang diinginkan saat kursor mendekati tombol

    private void Start()
    {
        // Simpan warna normal tombol saat inisialisasi
        normalColor = GetComponent<Image>().color;
    }

    // Implementasi dari antarmuka IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ganti warna saat kursor masuk
        GetComponent<Image>().color = hoverColor;
    }

    // Implementasi dari antarmuka IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
        // Kembalikan warna normal saat kursor keluar
        GetComponent<Image>().color = normalColor;
    }
}
