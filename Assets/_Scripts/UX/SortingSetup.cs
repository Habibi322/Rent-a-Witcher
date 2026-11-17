using UnityEngine;

public class SortingSetup : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            // ¬ключаем сортировку по пользовательской оси
            cam.transparencySortMode = TransparencySortMode.CustomAxis;
            // ”казываем, что сортировать нужно по Y (высоте)
            cam.transparencySortAxis = new Vector3(0f, 1f, 0f);
        }
    }
}