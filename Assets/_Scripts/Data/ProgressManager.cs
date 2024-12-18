using UnityEngine;

namespace SpaceScavengers
{
    public class ProgressManager : MonoBehaviour
    {
        private const string ProgressKey = "PlayerProgress";

        private static ProgressManager instance;

        private PlayerProgress _progress;
        public PlayerProgress Progress => _progress;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this);
            LoadProgress();
        }

        public static ProgressManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("ProgressManager instance not found!");
                }
                return instance;
            }
        }

        public void SaveProgress()
        {
            string json = JsonUtility.ToJson(_progress);
            PlayerPrefs.SetString(ProgressKey, json);
            PlayerPrefs.Save();
            Debug.Log("Progress saved!");
        }

        public void LoadProgress()
        {
            if (PlayerPrefs.HasKey(ProgressKey))
            {
                string json = PlayerPrefs.GetString(ProgressKey);
                _progress = JsonUtility.FromJson<PlayerProgress>(json);
                Debug.Log("Progress loaded!");
            }
            else
            {
                _progress = new PlayerProgress();
            }
        }

        public void UpdateCurrentLevel(int level)
        {
            if (_progress.currentLevel == level)
                _progress.currentLevel++;
        }

        public void UpdateTotalScore(int score) => _progress.totalScore += score;

        private void OnApplicationQuit()
        {
            SaveProgress();
        }
    }

    [System.Serializable]
    public class PlayerProgress
    {
        public int currentLevel;
        public int totalScore;
    }
}