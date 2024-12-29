using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GPHive
{
    public static class GameSettings
    {
        public const string SETTINGSFILEPATH = "SceneSettings";

        public static SceneSettings LoadSceneSettings
        {
            get
            {
                SceneSettings sceneSettings = Resources.Load<SceneSettings>(SETTINGSFILEPATH);
                if (sceneSettings)
                    return sceneSettings;
                return null;
            }
        }
    }
}
