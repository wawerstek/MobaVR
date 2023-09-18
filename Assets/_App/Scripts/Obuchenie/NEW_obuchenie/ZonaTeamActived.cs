using UnityEngine;

public class ZonaTeamActived : MonoBehaviour
{
    public Razvod_Po_Komandam razvodScript; // Ссылка на скрипт Razvod_Po_Komandam

    private void OnEnable() // Метод вызывается при активации объекта
    {
        if (razvodScript != null) // Проверяем, что ссылка на скрипт задана
        {
            razvodScript.RunZona(); // Вызываем метод RunZona из другого скрипта
        }
    }
}