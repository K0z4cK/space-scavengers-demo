using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScavengers
{
    public class LevelController : MonoBehaviour
    {
        private const string MenuSceneName = "Menu";

        [SerializeField] private UIManager _uiManager;
        [SerializeField] private Player _player;

        [SerializeField] private int _startPlayerHealth = 3;

        private int _score = 0;

        private bool _isLose = false;

        private void Awake()
        {
            _uiManager.Init(_player.StartAccelerate, _player.StopAccelerate, _player.StartBrake, _player.StopBrake, _player.Shoot);

            _player.Init(_startPlayerHealth, PlayerHit);
        }

        private void AddScoreForAsteroid(int score)
        {
            _score += score;
            //_uiManager.UpdateScore(_score);
        }

        private void PlayerHit(int health)
        {
            //_uiManager.UpdateLives(health);
            /*if (health <= 0)
            {
                OnLevelLose();
            }*/
        }
    }
}