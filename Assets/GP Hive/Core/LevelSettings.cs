using UnityEngine;
using UnityEngine.Rendering;

namespace GPHive.Core
{
    public class LevelSettings : Singleton<LevelSettings>
    {
        public LevelConfig levelConfig;
        public Color[] colors;
        [SerializeField] bool tutorialLevel;
        public enum ColorEnum
        {
            Orange, Blue, Purple, Red, Green, Pink
        }

        private void OnEnable()
        {
            /*if (levelConfig.enableVolume)
                FindObjectOfType<Volume>().profile = levelConfig.volumeProfile;
                */


            if (levelConfig.changeSkybox)
                RenderSettings.skybox = levelConfig.skybox;

            RenderSettings.fog = levelConfig.fog;

            if (levelConfig.fog)
            {
                RenderSettings.fogColor = levelConfig.fogColor;

                if (levelConfig.fogMode == FogMode.Linear)
                {
                    RenderSettings.fogStartDistance = levelConfig.fogStart;
                    RenderSettings.fogEndDistance = levelConfig.fogEnd;
                }
                else
                    RenderSettings.fogDensity = levelConfig.fogDensity;
            }
            if (tutorialLevel)
            {
                GameManager.Instance.TutorialCanvas.SetActive(true);
                Invoke("tutorialCanvasCloser", 10);
            }

        }
        void tutorialCanvasCloser()
        {
            GameManager.Instance.TutorialCanvas.SetActive(false);
        }

        private void Reset()
        {
            levelConfig.fogMode = FogMode.Linear;
        }
    }
}