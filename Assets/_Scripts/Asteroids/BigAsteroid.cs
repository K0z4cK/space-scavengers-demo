using UnityEngine;
using UnityEngine.Events;

namespace SpaceScavengers
{
    public class BigAsteroid : Asteroid
    {
        private event UnityAction<bool, BigAsteroid> OnBigAsteroidDestory;

        public void Init(Vector2 force, UnityAction<bool, BigAsteroid> onBigAsteroidDestory)
        {
            Init(force);
            OnBigAsteroidDestory = onBigAsteroidDestory;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(ProjectileTag))
            {
                ProjectilesController.instance.RemoveProjectile(collision.transform);

                OnBigAsteroidDestory?.Invoke(true, this);
                Destroy(gameObject);
            }
            else if (collision.CompareTag(PlayerTag))
            {
                collision.GetComponent<Player>().GetHit();

                OnBigAsteroidDestory?.Invoke(false, this);
                Destroy(gameObject);
            }
        }
    }
}