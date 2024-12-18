using UnityEngine;

namespace SpaceScavengers
{
    public abstract class Asteroid : MonoBehaviour
    {
        protected const string ProjectileTag = "Projectile";
        protected const string PlayerTag = "Player";

        private Rigidbody _rigidbody;

        protected Vector2 _force;
        public Vector2 Force => _force;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(Vector2 force)
        {
            _rigidbody.AddForce(force);
            _force = force;
        }
    }
}