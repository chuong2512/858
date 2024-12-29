using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GPHive.Game;

namespace GPHive.Core
{
    public enum GameState
    {
        Idle,
        Playing,
        End
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private TextMeshProUGUI levelText;

        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;
        public GameObject TutorialCanvas;

        [SerializeField] private float OpenFinalUIAfterTime;

        private GameState gameState;
        public GameState GameState { get { return gameState; } }

        private void Start()
        {
            SetLevelText();
        }

        private void SetLevelText()
        {
            levelText.SetText($"LEVEL {SaveLoadManager.GetLevel()}");
        }

        public void StartLevel()
        {
            EventManager.StartLevel(SaveLoadManager.GetLevel());

            gameState = GameState.Playing;
        }

        public void NextLevel()
        {
            ObjectPooling.Instance.DepositAll();
            StartCoroutine(CO_NextLevel());
        }

        IEnumerator CO_NextLevel()
        {
            Destroy(LevelManager.Instance.ActiveLevel);
            yield return new WaitForEndOfFrame();
            LevelManager.Instance.ActiveLevel = LevelManager.Instance.CreateLevel();

            gameState = GameState.Idle;

            SetLevelText();
        }

        /// <summary>
        /// Call when level is successfully finished.
        /// </summary>
        public void WinLevel()
        {
            EventManager.SuccessLevel(SaveLoadManager.GetLevel());
            SaveLoadManager.IncreaseLevel();

            gameState = GameState.End;

            StartCoroutine(CO_OpenUIDelayed(winScreen));
        }

        /// <summary>
        /// Call when level is failed.
        /// </summary>
        public void LoseLevel()
        {
            EventManager.FailLevel(SaveLoadManager.GetLevel());

            gameState = GameState.End;

            StartCoroutine(CO_OpenUIDelayed(loseScreen));
        }

        IEnumerator CO_OpenUIDelayed(GameObject UI)
        {
            yield return BetterWaitForSeconds.Wait(OpenFinalUIAfterTime);
            UI.SetActive(true);
        }
    }
}
