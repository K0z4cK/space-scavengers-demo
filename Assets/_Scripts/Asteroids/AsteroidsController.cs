using UnityEngine;
using UnityEngine.Events;

namespace SpaceScavengers
{
    public class AsteroidsController : MonoBehaviour
    {
        private event UnityAction OnBigAsteroidDestroy;
        private event UnityAction OnSmallAsteroidDestroy;
        private event UnityAction OnAllAsteroidsDestroy;

        [SerializeField] private BigAsteroid _bigAsteroidPrefab;
        [SerializeField] private SmallAsteroid _smallAsteroidPrefab;

        [SerializeField] private float _spawnDistance = 10f;

        private int _asteroidsCount;
        private float _minSpeed;
        private float _maxSpeed;

        private int _bigAsteroidsLeft = 0;
        private int _smallAsteroidsLeft = 0;

        private Vector2 _screenBounds;

        public void Init(LevelDataScriptableObject levelData, UnityAction onBigAsteroidDestroy, UnityAction onSmallAsteroidDestroy, UnityAction onAllAsteroidsDestroy)
        {
            OnBigAsteroidDestroy = onBigAsteroidDestroy;
            OnSmallAsteroidDestroy = onSmallAsteroidDestroy;
            OnAllAsteroidsDestroy = onAllAsteroidsDestroy;

            _asteroidsCount = levelData.asteroidsCount;
            _minSpeed = levelData.minAsteroidSpeed;
            _maxSpeed = levelData.maxAsteroidSpeed;

            Camera cam = Camera.main;
            _screenBounds = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);

            for (int i = 0; i < _asteroidsCount; i++)
            {
                SpawnAsteroid();
            }
        }

        public void SpawnAsteroid()
        {
            Vector2 spawnPosition = GetRandomPositionOutsideScreen();

            var asteroid = Instantiate(_bigAsteroidPrefab, spawnPosition, Quaternion.identity);

            Vector2 targetDirection = GetDirectionTowardsScreen(spawnPosition);
            float randomSpeed = Random.Range(_minSpeed, _maxSpeed);

            asteroid.Init(targetDirection.normalized * randomSpeed, BigAsteroidDestroy);
            _bigAsteroidsLeft++;
        }

        private Vector2 GetRandomPositionOutsideScreen()
        {
            float side = Random.Range(0f, 4f);
            Vector2 position;

            if (side < 1f)
                position = new Vector2(-_screenBounds.x - _spawnDistance, Random.Range(-_screenBounds.y, _screenBounds.y));
            else if (side < 2f)
                position = new Vector2(_screenBounds.x + _spawnDistance, Random.Range(-_screenBounds.y, _screenBounds.y));
            else if (side < 3f)
                position = new Vector2(Random.Range(-_screenBounds.x, _screenBounds.x), -_screenBounds.y - _spawnDistance);
            else
                position = new Vector2(Random.Range(-_screenBounds.x, _screenBounds.x), _screenBounds.y + _spawnDistance);

            return position;
        }

        private Vector2 GetDirectionTowardsScreen(Vector2 position)
        {
            Vector2 randomTarget = new Vector2(
                Random.Range(-_screenBounds.x, _screenBounds.x),
                Random.Range(-_screenBounds.y, _screenBounds.y)
            );
            return randomTarget - position;
        }

        private void SpawnSmallAsteroid(Vector2 postion, Vector2 force)
        {
            var smallAsteroid = Instantiate(_smallAsteroidPrefab, postion, _smallAsteroidPrefab.transform.rotation);
            smallAsteroid.Init(force, SmallAsteroidDestroy);
            _smallAsteroidsLeft++;
        }

        private void BigAsteroidDestroy(bool isBeenShot, BigAsteroid asteroid)
        {
            _bigAsteroidsLeft--;

            if (isBeenShot)
            {
                Vector2 dividedForce = asteroid.Force / 2f;

                SpawnSmallAsteroid(asteroid.transform.position, dividedForce);
                SpawnSmallAsteroid(asteroid.transform.position, -dividedForce);

                OnBigAsteroidDestroy?.Invoke();
            }
        }

        private void SmallAsteroidDestroy(bool isBeenShot)
        {
            _smallAsteroidsLeft--;

            if (isBeenShot)
                OnSmallAsteroidDestroy?.Invoke();

            if (_bigAsteroidsLeft == 0 && _smallAsteroidsLeft == 0)
                OnAllAsteroidsDestroy?.Invoke();
        }
    }
}