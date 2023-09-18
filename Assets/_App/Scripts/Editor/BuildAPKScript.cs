using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildAPKScript 
{
    [MenuItem("BuildAPK/Build APK for Arma")]
    public static void BuildApkForArma()
    {
        BuildCommon.BuildForCity("Arma", BuildTarget.Android, ".apk");
    }

    [MenuItem("BuildAPK/Build APK for China")]
    public static void BuildApkForChina()
    {
        BuildCommon.BuildForCity("China", BuildTarget.Android, ".apk");
    }
}