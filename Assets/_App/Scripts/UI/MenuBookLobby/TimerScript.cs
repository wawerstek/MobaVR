using UnityEngine;
using UnityEngine.UI;  // Для работы с компонентом Text

public class TimerScript : MonoBehaviour
{
    public float timerDuration = 100f;  // Продолжительность таймера в секундах
    private float timerLeft;

    public Text timerText;  // Ссылка на текстовый компонент, где будет отображаться таймер

    public SliderMenu sliderMenuScript;  // Ссылка на скрипт SliderMenu

    private void Start()
    {
        timerDuration = 100f;
        timerLeft = timerDuration;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        timerLeft -= Time.deltaTime;  // Уменьшаем оставшееся время
        UpdateTimerDisplay();

        if (timerLeft <= 0)
        {
            TimerFinished();
        }
    }

    private void UpdateTimerDisplay()
    {
        timerText.text = Mathf.CeilToInt(timerLeft).ToString();  // Округляем вверх и преобразуем в строку
    }

    private void TimerFinished()
    {
        sliderMenuScript.NextClickName();  // Вызываем функцию из другого скрипта
        
    }
}
