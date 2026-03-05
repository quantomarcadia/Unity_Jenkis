using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using System;
using System.IO.Compression;
using UnityEngine;

public class BuildScript
{
    // ===========================
    // WINDOWS BUILD
    // ===========================
    public static void BuildWindows()
    {
        // Switch platform if needed
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Standalone,
                BuildTarget.StandaloneWindows64
            );
        }

        string folderPath = "Builds/Windows";
        CreateDirectory(folderPath);

        string exePath = Path.Combine(folderPath, "MyGame.exe");

        if (File.Exists(exePath))
            File.Delete(exePath);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = exePath,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            ZipBuild(folderPath);
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError("Windows Build Failed");
            EditorApplication.Exit(1);
        }
    }


    // ===========================
    // ANDROID BUILD (APK / AAB)
    // ===========================
    public static void BuildAndroid()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Android,
                BuildTarget.Android
            );
        }

        string[] args = Environment.GetCommandLineArgs();
        string buildType = GetArgument(args, "-buildType");

        if (string.IsNullOrEmpty(buildType))
        {
            Debug.LogError("Missing -buildType argument (APK or AAB)");
            EditorApplication.Exit(1);
            return;
        }

        bool isAAB = buildType.ToUpper() == "AAB";

        string folderPath = isAAB ? "Builds/AndroidAAB" : "Builds/AndroidAPK";
        string fileName = isAAB ? "MyGame.aab" : "MyGame.apk";

        CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        EditorUserBuildSettings.buildAppBundle = isAAB;

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = fullPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError("Android Build Failed");
            EditorApplication.Exit(1);
        }
    }


    // ===========================
    // LINUX HEADLESS SERVER
    // ===========================
    public static void BuildLinuxServer()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneLinux64)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Standalone,
                BuildTarget.StandaloneLinux64
            );
        }

        string folderPath = "Builds/LinuxServer";
        CreateDirectory(folderPath);

        string serverPath = Path.Combine(folderPath, "MyServer.x86_64");

        if (File.Exists(serverPath))
            File.Delete(serverPath);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = GetEnabledScenes(),
            locationPathName = serverPath,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.EnableHeadlessMode
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError("Linux Server Build Failed");
            EditorApplication.Exit(1);
        }
    }


    // ===========================
    // HELPER METHODS
    // ===========================

    private static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private static string[] GetEnabledScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static void ZipBuild(string folderPath)
    {
        string zipPath = folderPath + ".zip";

        if (File.Exists(zipPath))
            File.Delete(zipPath);

        ZipFile.CreateFromDirectory(folderPath, zipPath);
    }

    private static string GetArgument(string[] args, string name)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && i + 1 < args.Length)
                return args[i + 1];
        }
        return null;
    }
}