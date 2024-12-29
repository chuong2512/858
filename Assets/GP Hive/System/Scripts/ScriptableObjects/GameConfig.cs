#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace GPHive
{
    public class GameConfig : ScriptableObject
    {
        private const string CONFIGSFILEPATH = "GameConfigs";

        [Header("Company Settings")]
        public string companyName;
        public string teamID;

        [Header("Product Settings")]
        public string productName;
        public string bundleID;
        public string version;
        public int buildNo;

        [Header("Icon Settings")]
        public Texture2D icon;

        public static GameConfig GetGameConfigs()
        {
            return Resources.Load<GameConfig>(CONFIGSFILEPATH);
        }


        public GameConfig(GameConfig gameConfig)
        {
            this.companyName = gameConfig.companyName;
            this.teamID = gameConfig.teamID;
            this.productName = gameConfig.productName;
            this.bundleID = gameConfig.bundleID;
            this.version = gameConfig.version;
            this.buildNo = gameConfig.buildNo;
            this.icon = gameConfig.icon;
        }

#if UNITY_EDITOR

        public void ManualValidate()
        {
            PlayerSettings.companyName = companyName;
            PlayerSettings.bundleVersion = version;
            PlayerSettings.iOS.buildNumber = buildNo.ToString();
            PlayerSettings.productName = productName;
            PlayerSettings.iOS.appleDeveloperTeamID = teamID;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleID);
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, new Texture2D[] { icon });
        }

        [MenuItem("Build/Game Settings/Game Configs", false, 200)]
        public static void Settings()
        {
            var settings = Resources.Load<GameConfig>(CONFIGSFILEPATH);
            if (settings != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settings;
            }
        }
#endif
    }
}
