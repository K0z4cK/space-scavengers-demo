using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScavengers
{
    public class MenuController : MonoBehaviour
    {
        private const string GameSceneName = "Game";

        [SerializeField] private LevelButton _levelButtonPrefab;

        [SerializeField] private Transform _buttonsContent;

        private int _levelsCount;
        private int _currentLevel;

        private void Start()
        {
            _levelsCount = LevelDataHolder.Instance.LevelDatas.Count;
            _currentLevel = ProgressManager.Instance.Progress.currentLevel;

            SpawnButtons();
        }

        private void SpawnButtons()
        {
            for (int i = 0; i < _levelsCount; i++)
            {
                var newButton = Instantiate(_levelButtonPrefab, _buttonsContent);

                bool isOpen = i <= _currentLevel;

                newButton.Init(i, isOpen, LoadLevel);
            }
        }

        private void LoadLevel(int i)
        {
            LevelDataHolder.Instance.SetCurrentLevelIndex(i);
            SceneManager.LoadScene(GameSceneName);
        }
    }
}