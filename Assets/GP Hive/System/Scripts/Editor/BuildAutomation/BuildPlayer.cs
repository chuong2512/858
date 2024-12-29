using UnityEditor;
using UnityEngine;
using UnityEditor.Build.Reporting;

// Output the build size or a failure depending on BuildPlayer.

namespace GPHive.System.Editor
{
    public class BuildPlayer : MonoBehaviour
    {
        [MenuItem("Build/iOS Release", false, 0)]
        public static void BuildiOSRelease()
        {
            GameConfig gameConfig = GameConfig.GetGameConfigs();

            if (gameConfig)
                gameConfig.ManualValidate();
            else
                Debug.Log("Game Config is not set");

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GameSettings.LoadSceneSettings.GetScenesPaths();
            buildPlayerOptions.locationPathName = "Builds/iOSBuild";
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder("Builds/iOSBuild");
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");

                if (gameConfig)
                {
                    gameConfig.buildNo++;

                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(gameConfig);
                }
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        [MenuItem("Build/iOS Debug", false, 1)]
        public static void BuildiOSDebug()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

            buildPlayerOptions.scenes = GameSettings.LoadSceneSettings.GetScenesPaths();
            buildPlayerOptions.locationPathName = "Builds/iOSDebug";
            buildPlayerOptions.target = BuildTarget.iOS;
            buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                EditorUtility.RevealInFinder("Builds/iOSDebug");
                Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            }

            if (summary.result == BuildResult.Failed)
            {
                Debug.Log("Build failed");
            }
        }

        [MenuItem("Build/Open Builds Folder", false, 100)]
        public static void OpenBuildsFolder()
        {
#if UNITY_EDITOR_OSX
            EditorUtility.RevealInFinder("Builds");
#elif UNITY_EDITOR_WIN
        // Open in File Explorer.
#endif
        }
    }
}