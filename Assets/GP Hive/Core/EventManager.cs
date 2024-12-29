using UnityEngine;
using ElephantSDK;

namespace GPHive.Core
{
    public class EventManager : MonoBehaviour
    {
        public delegate void OnLevelStart();
        public static event OnLevelStart LevelStarted;

        public delegate void OnLevelSuccess();
        public static event OnLevelSuccess LevelSuccessed;

        public delegate void OnLevelFail();
        public static event OnLevelFail LevelFailed;

        public delegate void OnLevelRestart();
        public static event OnLevelRestart LevelRestarted;

        public delegate void OnNextLevelCreated();
        public static event OnNextLevelCreated NextLevelCreated;


        public static void StartLevel(int level)
        {
            LevelStarted?.Invoke();
            Elephant.LevelStarted(level);
        }

        public static void SuccessLevel(int level)
        {
            LevelSuccessed?.Invoke();
            Elephant.LevelCompleted(level);
        }

        public static void FailLevel(int level)
        {
            LevelFailed?.Invoke();
            Elephant.LevelFailed(level);
        }

        public static void RestartLevel(int level)
        {
            LevelRestarted?.Invoke();
            Elephant.LevelStarted(level);
        }

        public static void NextLevel()
        {
            NextLevelCreated?.Invoke();
        }
    }
}
