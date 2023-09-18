using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildEXEScript 
{
    [MenuItem("BuildEXE/Build EXE for Arma")]
    public static void BuildExeForArma()
    {
        BuildCommon.BuildForCity("Arma", BuildTarget.StandaloneWindows64, ".exe");
    }

    [MenuItem("BuildEXE/Build EXE for China")]
    public static void BuildExeForChina()
    {
        BuildCommon.BuildForCity("China", BuildTarget.StandaloneWindows64, ".exe");
    }
}