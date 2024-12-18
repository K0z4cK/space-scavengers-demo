using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceScavengers
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private event UnityAction<int> OnPlayerHit;

        [Header("Controll")]
        [SerializeField] private VariableJoystick _variableJoystick;
        [SerializeField] private float _accelerateSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _tiltAmount;
        [SerializeField] private float _tiltSpeed;

        [Header("Projectiles")]
        [SerializeField] private float _projectileForce;
        [SerializeField] private Transform _projectilesSpawnPosition;

        [Header("Shield")]
        [SerializeField] private float _shieldTime;
        [SerializeField] private string _animatorShieldBoolKey = "IsShieldActivated";

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Animator _animator;

        private int _healthCount;

        private bool _isAccelerating = false;
        private bool _isShieldActivated = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _transform = transform;
        }

        public void Init(int healthCount, UnityAction<int> onPlayerHit)
        {
            _healthCount = healthCount;
            OnPlayerHit = onPlayerHit;
        }

        public void StartAccelerate() => _isAccelerating = true;

        public void StopAccelerate() => _isAccelerating = false;

        public void Accelerate()
        {
            _rigidbody.AddForce(_transform.forward * _accelerateSpeed);
        }

        public void Rotate()
        {
            Vector2 direction = new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical);

            if (direction.sqrMagnitude <= 0) return;

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.y;

            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.fixedDeltaTime * _rotateSpeed);

           /* float tilt = direction.x * _tiltAmount;

            float currentTilt = transform.eulerAngles.z;
            if (currentTilt > 180) currentTilt -= 360;

            float newTilt = Mathf.Lerp(currentTilt, -tilt, Time.fixedDeltaTime * _tiltSpeed);*/

            _rigidbody.MoveRotation(Quaternion.Euler(0, newAngle, HandleTilt(direction)));
        }

        private float HandleTilt(Vector2 direction)
        {
            float currentTilt = 0;

            if (direction.sqrMagnitude <= 0)
            {
                currentTilt = transform.eulerAngles.z;
                //if (currentTilt > 180) currentTilt -= 360;

                return Mathf.Lerp(currentTilt, 0, Time.fixedDeltaTime * _tiltSpeed);
            }

            float tiltDirection = Mathf.Sign(direction.x);
            float targetTilt = tiltDirection * _tiltAmount;

            currentTilt = transform.eulerAngles.z;
            //if (currentTilt > 180) currentTilt -= 360;

            return Mathf.Lerp(currentTilt, targetTilt, Time.fixedDeltaTime * _tiltSpeed);
            //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newTilt);
        }

        public void Shoot()
        {
            ProjectilesController.instance.SpawnProjectile(_projectilesSpawnPosition.position, _transform.up * _projectileForce);
        }

        public void GetHit()
        {
            if (_isShieldActivated) return;

            _healthCount--;
            OnPlayerHit?.Invoke(_healthCount);
            StartCoroutine(ShieldCoroutine());

            Debug.Log("player hit");
        }

        private IEnumerator ShieldCoroutine()
        {
            _isShieldActivated = true;
            _animator.SetBool(_animatorShieldBoolKey, _isShieldActivated);
            yield return new WaitForSeconds(_shieldTime);
            _isShieldActivated = false;
            _animator.SetBool(_animatorShieldBoolKey, _isShieldActivated);
        }

        private void FixedUpdate()
        {
            Rotate();

            if (_isAccelerating)
                Accelerate();
        }
    }
}