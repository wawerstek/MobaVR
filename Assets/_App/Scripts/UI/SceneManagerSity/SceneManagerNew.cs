using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SceneManagerNew 
{
    [MenuItem("Build/Build APK for Moscow")]
    public static void BuildForMoscow()
    {
        BuildForCity("Moscow");
    }

    [MenuItem("Build/Build APK for China")]
    public static void BuildForChina()
    {
        BuildForCity("China");
    }

    private static void BuildForCity(string cityName)
    {
        string[] scenes = GetScenesForCity(cityName);
        if(scenes.Length == 0)
        {
            Debug.LogError("No scenes found for " + cityName);
            return;
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = "Builds/" + cityName + ".apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded for " + cityName);
        }
    }

    private static string[] GetScenesForCity(string cityName)
    {
        // Список всех карт, которые могут быть в любом городе.
        string[] baseMapNames = { "Taverna", "SkySity" };

        // Создаём массив строк, который будет содержать полные пути к сценам.
        string[] scenes = new string[baseMapNames.Length];

        for (int i = 0; i < baseMapNames.Length; i++)
        {
            // Формируем полный путь к сцене с использованием базового имени карты и имени города.
            scenes[i] = $"Assets/Maps/{cityName}/{baseMapNames[i]}_{cityName}.unity";
        }

        return scenes;
    }
}
