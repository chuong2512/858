using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SceneSettings", menuName = "ScriptableObjects/SceneSettings", order = 1)]
public class SceneSettings : ScriptableObject
{
    private const string SCENESFILEPATH = "SceneSettings";
    public SceneReference splashScene;
    public List<SceneReference> gameSceneList;

#if UNITY_EDITOR
    public string[] GetScenesPaths()
    {
        List<string> _scenePaths = new List<string>();

        _scenePaths.Add(splashScene.ScenePath);

        for (int i = 0; i < gameSceneList.Count; i++)
        {
            string _path = gameSceneList[i].ScenePath;
            if (!_scenePaths.Contains(_path) && gameSceneList[i].IsValidSceneAsset)
                _scenePaths.Add(_path);
        }

        return _scenePaths.ToArray();
    }

    [MenuItem("Build/Game Settings/Scene Settings", false, 201)]
    public static void Settings()
    {
        var settings = Resources.Load<SceneSettings>(SCENESFILEPATH);
        if (settings != null)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = settings;
        }
    }
#endif
}
