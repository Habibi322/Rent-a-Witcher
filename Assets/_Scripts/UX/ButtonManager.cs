using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Настройки")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private const string SliderPrefix = "Setting_Slider_";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnSettingsClicked()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);

        LoadAllSliders();
    }

    public void OnQuitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnBackToMenuClicked()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
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
}
