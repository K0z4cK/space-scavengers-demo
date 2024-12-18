using System.Collections.Generic;
using UnityEngine;

namespace SpaceScavengers
{
    public class LevelDataHolder : MonoBehaviour
    {
        private static LevelDataHolder instance;

        [SerializeField] private List<LevelDataScriptableObject> _levelDatas;

        public List<LevelDataScriptableObject> LevelDatas => _levelDatas;

        private int _currentLevelIndex;
        public int CurrentLevelIndex => _currentLevelIndex;
        public LevelDataScriptableObject CurrentLevelData => _levelDatas[_currentLevelIndex];

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this);
        }

        public static LevelDataHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.LogError("LevelDataHolder instance not found!");
                }
                return instance;
            }
        }

        public void SetCurrentLevelIndex(int index) => _currentLevelIndex = index;

        public bool IsLastLevelIndex() => _currentLevelIndex + 1 >= _levelDatas.Count;

        public void UpdateToNextLevelIndex() => _currentLevelIndex++;
    }
}