using UnityEngine;

[CreateAssetMenu(fileName = "AppSettingSity", menuName = "MyGame/AppSettingSity", order = 1)]
public class AppSettingSity : ScriptableObject
{
    //скрипт хранит имя города, которое нужно добавлять к названиям сцен на вызове в самой игре. Это имя ему приходит из скрипта для билда игры под город
    public string CurrentCity = "Arma";
}