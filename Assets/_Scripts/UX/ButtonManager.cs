//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Настройки")]
    [SerializeField] private string gameplaySceneName = "GameScene";
    [SerializeField] private ContextDefaults contextDefaults;

    private const string SliderPrefix = "Setting_Slider_";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OnPlayClicked()
    {
        // Сбрасываем все контексты в дефолтные значения
        if (contextDefaults != null && ContextBootstrapper.ContextSystem != null)
        {
            contextDefaults.ResetAll(ContextBootstrapper.ContextSystem);
            Debug.Log("Все контексты сброшены!");
        }

        // Загружаем игровую сцену
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnSettingsClicked()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(true);
        controlsPanel.SetActive(false);

        LoadAllSliders();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void OnBackToMenuClicked()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void OnConfirmClicked()
    {
        Slider[] allSliders = settingsPanel.GetComponentsInChildren<Slider>(true);

        foreach (Slider slider in allSliders)
        {
            string key = SliderPrefix + slider.gameObject.name;
            PlayerPrefs.SetFloat(key, slider.value);
        }

        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void OnControlsClicked()
    {
        // Скрываем главную панель
        if (mainPanel != null) mainPanel.SetActive(true);

        // Показываем панель настроек (управления)
        if (controlsPanel != null) controlsPanel.SetActive(true);

        // Загружаем сохраненные значения слайдеров (если они есть)
        LoadAllSliders();
    }
}
