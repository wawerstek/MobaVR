using UnityEngine;
using UnityEngine.UI;

public class PlayerMenuButtonScript : MonoBehaviour
{
   // public string baseSceneName; // Название сцены, которое будет установлено в инспекторе для каждой кнопки
    
    public void StartScena(string baseSceneName)
    {
        CityManager.Instance.LoadCityScene(baseSceneName);
    }
}