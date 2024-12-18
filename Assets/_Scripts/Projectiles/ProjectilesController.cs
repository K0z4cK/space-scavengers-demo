using UnityEngine;
using UnityEngine.Pool;

namespace SpaceScavengers
{
    public class ProjectilesController : MonoBehaviour
    {
        public static ProjectilesController instance;

        [SerializeField] private Transform _projectilePrefab;

        private ObjectPool<Transform> _projectilesPool;

        private void Awake()
        {
            instance = this;

            _projectilesPool = new ObjectPool<Transform>(Create, Get, Release);
        }

        public void SpawnProjectile(Vector2 position, Vector2 force)
        {
            Transform projectile = _projectilesPool.Get();
            projectile.position = position;
            projectile.GetComponent<Rigidbody2D>().AddForce(force);
        }

        public void RemoveProjectile(Transform projectile)
        {
            if (projectile != null && projectile.gameObject.activeSelf)
            {
                _projectilesPool.Release(projectile);
            }
        }

        private Transform Create()
        {
            var projectile = Instantiate(_projectilePrefab, transform);
            projectile.gameObject.SetActive(false);
            return projectile;
        }

        private void Get(Transform projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private void Release(Transform projectile)
        {
            projectile.gameObject.SetActive(false);
        }
    }
}