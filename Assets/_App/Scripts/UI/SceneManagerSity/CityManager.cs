using UnityEngine;
using UnityEngine.SceneManagement; // Для работы со сценами.

public class CityManager : MonoBehaviour
{
    // Пусть это будет синглтон, чтобы вы могли легко получить доступ к нему из любой части вашего приложения.
    public static CityManager Instance;

    // Название текущего города.
    public string CurrentCity = "Moscow"; // По умолчанию Moscow, но вы можете менять это значение в зависимости от билда.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Объект останется живым при переключении сцен.
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Функция для загрузки сцены на основе названия сцены и текущего города.
    public void LoadCityScene(string baseSceneName)
    {
        string sceneName = $"{baseSceneName}_{CurrentCity}";
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}