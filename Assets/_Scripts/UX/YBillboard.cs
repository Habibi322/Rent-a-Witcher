using UnityEngine;

public class YBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Получаем текущий поворот камеры
        Vector3 cameraEuler = Camera.main.transform.eulerAngles;

        // Берем ТОЛЬКО поворот по горизонтали (Y), а наклон камеры (X) и крен (Z) игнорируем
        // Значение 0 по X и Z гарантирует, что спрайт будет стоять строго вертикально
        transform.rotation = Quaternion.Euler(0f, cameraEuler.y, 0f);
    }
}