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
    string buildType = GetArgument(args, "-buildType");

    bool isAAB = buildType == "AAB";

    string folderPath = isAAB ? "Builds/AndroidAAB" : "Builds/AndroidAPK";
    string fileName = isAAB ? "MyGame.aab" : "MyGame.apk";

    CreateDirectory(folderPath);

    string fullPath = Path.Combine(folderPath, fileName);

    if (File.Exists(fullPath))
        File.Delete(fullPath);

    EditorUserBuildSettings.buildAppBundle = isAAB;

    BuildPlayerOptions options = new BuildPlayerOptions
    {
        scenes = GetEnabledScenes(),
        locationPathName = fullPath,
        target = BuildTarget.Android,
        options = BuildOptions.None
    };

    BuildPipeline.BuildPlayer(options);
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