using UnityEngine;
using UnityEngine.Events;

namespace SpaceScavengers
{
    public class SmallAsteroid : Asteroid
    {
        private event UnityAction<bool> OnSmallAsteroidDestory;

        public void Init(Vector2 force, UnityAction<bool> onSmallAsteroidDestory)
        {
            Init(force);
            OnSmallAsteroidDestory = onSmallAsteroidDestory;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag(ProjectileTag))
            {
                ProjectilesController.instance.RemoveProjectile(collision.transform);

                OnSmallAsteroidDestory?.Invoke(true);
                Destroy(gameObject);
            }
            else if (collision.CompareTag(PlayerTag))
            {
                collision.GetComponent<Player>().GetHit();

                OnSmallAsteroidDestory?.Invoke(false);
                Destroy(gameObject);
            }
        }
    }
}