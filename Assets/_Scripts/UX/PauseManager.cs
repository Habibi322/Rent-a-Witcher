using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private GameObject pausePanel;    // Сама панель паузы
    [SerializeField] private GameObject settingsPanel; // Панель настроек внутри паузы
    [SerializeField] private PlayerInput playerInput;  // Ссылка на ваш Input

    [Header("Настройки")]
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Имя сцены главного меню

    private bool _isPaused = false;
    private const string SliderPrefix = "Setting_Slider_";

    private void Awake()
    {
        // Подписываемся на нажатие Esc
        playerInput.OnPausePressed += HandlePauseInput;
    }

    private void OnDestroy()
    {
        // Отписываемся
        playerInput.OnPausePressed -= HandlePauseInput;
    }

    private void Start()
    {
        // При старте игры панели скрыты
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void HandlePauseInput()
    {
        // Если открыты настройки, Esc закрывает их и возвращает в паузу
        if (settingsPanel.activeSelf)
        {
            CloseSettings();
            return;
        }

        // Иначе переключаем паузу
        if (_isPaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        _isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Останавливаем время
        // Отключаем управление игроком (если нужно)
        playerInput.enabled = false;
    }

    public void ResumeGame()
    {
        _isPaused = false;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // Возвращаем время
        playerInput.enabled = true;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
        LoadAllSliders(); // Загружаем ползунки
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        SaveAllSliders(); // Сохраняем при выходе
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Обязательно сбрасываем время перед загрузкой!
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // --- Логика слайдеров (взята из вашего скрипта) ---
    public void SaveAllSliders()
    {
        Slider[] allSliders = settingsPanel.GetComponentsInChildren<Slider>(true);
        foreach (Slider slider in allSliders)
        {
            string key = SliderPrefix + slider.gameObject.name;
            PlayerPrefs.SetFloat(key, slider.value);
        }
        PlayerPrefs.Save();
    }

    private void LoadAllSliders()
    {
        Slider[] allSliders = settingsPanel.GetComponentsInChildren<Slider>(true);
        foreach (Slider slider in allSliders)
        {
            string key = SliderPrefix + slider.gameObject.name;
            float saveValue = PlayerPrefs.GetFloat(key, slider.value);
            slider.SetValueWithoutNotify(saveValue);
        }
    }
}