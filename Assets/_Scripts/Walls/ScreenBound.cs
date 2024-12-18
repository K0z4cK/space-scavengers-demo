using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceScavengers
{
    public class ScreenBound : MonoBehaviour
    {
        private event UnityAction<Transform> onAddToIgnoreList;

        private const string PlayerTag = "Player";
        private const string AsteroidTag = "Asteroid";
        private const string ProjectileTag = "Projectile";

        private bool _isHorizontal;
        private List<Transform> _ignoreTransforms = new List<Transform>();
        private Transform _transform;
        private Vector2 _colliderOffset;

        private void Awake()
        {
            _transform = transform;
            //_colliderOffset = GetComponent<BoxCollider>().
        }

        public void Init(bool isHorizontal, UnityAction<Transform> addToIgnoreList)
        {
            _isHorizontal = isHorizontal;
            onAddToIgnoreList += addToIgnoreList;
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (_ignoreTransforms.Contains(collision.transform))
                return;

            if (_isHorizontal && Mathf.Abs(_transform.position.x + _colliderOffset.x) < Mathf.Abs(collision.transform.position.x))
                return;

            if (!_isHorizontal && Mathf.Abs(_transform.position.y + _colliderOffset.y) < Mathf.Abs(collision.transform.position.y))
                return;

            if (collision.CompareTag(PlayerTag) || collision.CompareTag(AsteroidTag))
            {
                onAddToIgnoreList?.Invoke(collision.transform);

                if (_isHorizontal)
                    collision.transform.position = new Vector2(-collision.transform.position.x, collision.transform.position.y);
                else
                    collision.transform.position = new Vector2(collision.transform.position.x, -collision.transform.position.y);

            }

            else if (collision.CompareTag(ProjectileTag))
            {
                ProjectilesController.instance.RemoveProjectile(collision.transform);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (_ignoreTransforms.Contains(collision.transform))
                _ignoreTransforms.Remove(collision.transform);
        }

        public void AddToIgnoreList(Transform transform) => _ignoreTransforms.Add(transform);
    }
}