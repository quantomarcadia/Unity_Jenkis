using System.IO;
using UnityEditor;
using System.Linq;
using System.IO.Compression;
using System;

public class BuildScript
{
    public static void BuildWindows()
    {
        string path = "Builds/Windows";
        CreateDirectory(path);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = $"{path}/MyGame.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
        ZipBuild(path);
    }
    
    public static void BuildAndroid()
    {
        string[] args = Environment.GetCommandLineArgs();
        string builType = GetArgument(args, "-buildType");

        string path;
        if (builType == "APK")
        {
            path = "Builds/AndroidAPK/MyGame.apk";
        }
        else if (builType == "AAB")
        {
            path = "Builds/AndroidAAB/MyGame.aab";
        }
        else
        {
            return;
        }

        CreateDirectory(path);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = $"{path}",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        PlayerSettings.Android.minSdkVersion = (AndroidSdkVersions)25;
        PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)34;
        EditorUserBuildSettings.buildAppBundle = (builType == "AAB") ? true : false;

        BuildPipeline.BuildPlayer(buildPlayerOptions);

    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))   
        {
            Directory.CreateDirectory(path);
        }
    }

    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static void ZipBuild(string buildPath)
    {
        string zipPath = buildPath + ".zip";

        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }

        ZipFile.CreateFromDirectory(buildPath, zipPath);
    }

    private static string GetArgument(string[] args, string name)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && i + 1 < args.Length)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}