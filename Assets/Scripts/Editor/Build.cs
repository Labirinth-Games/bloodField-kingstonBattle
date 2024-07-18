using System;
using UnityEditor;

public class Build
{
    [MenuItem("Build/Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/ServerScene.unity" };
        buildPlayerOptions.locationPathName = "build/Linux/Servers/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC; // | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building server (linux)..");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Building Finish (linux).");
    }

    [MenuItem("Build/Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/LobbyScene.unity", "Assets/Scenes/GameScene.unity" };
        buildPlayerOptions.locationPathName = "build/Windows/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        Console.WriteLine("Building server (linux)..");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Building Finish (linux).");
    }
}