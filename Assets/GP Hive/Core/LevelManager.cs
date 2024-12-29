using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPHive.Core
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private GameObject[] allLevels;
        [SerializeField] private int tutorialLevels;
        [SerializeField] private bool enableTestLevel;
        [ShowIf("enableTestLevel")][SerializeField] private GameObject testLevel;
        [SerializeField] private Transform levelHolder;

        public GameObject ActiveLevel { get; set; }

        private void Start()
        {
            ActiveLevel = CreateLevel();
        }

        /// <summary>
        /// Creates test level if exists, otherwise creates next level.
        /// </summary>
        /// <returns>Created level object</returns>
        public GameObject CreateLevel()
        {
            int _nextLevel = 0;

            if (SaveLoadManager.GetLevel() > allLevels.Length)
                _nextLevel = (SaveLoadManager.GetLevel() - 1) % (allLevels.Length - tutorialLevels) + tutorialLevels;
            else
                _nextLevel = (SaveLoadManager.GetLevel() - 1) % allLevels.Length;

#if UNITY_EDITOR
            GameObject level = Instantiate(enableTestLevel ? testLevel : allLevels[_nextLevel], levelHolder);
#else
            GameObject level = Instantiate(allLevels[_nextLevel], levelHolder);
#endif
            EventManager.NextLevel();
            return level;
        }

        public static void RestartScene() => SceneManager.LoadScene("Game");
    }
}
