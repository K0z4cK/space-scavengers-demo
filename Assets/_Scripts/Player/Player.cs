using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VSX.UniversalVehicleCombat;

namespace SpaceScavengers
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator))]
    public class Player : MonoBehaviour
    {
        private event UnityAction<int> OnPlayerHit;

        [Header("Controll")]
        [SerializeField] private VariableJoystick _variableJoystick;
        [SerializeField] private float _accelerateForce;
        [SerializeField] private float _brakeForce;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _tiltAmount;
        [SerializeField] private float _tiltSpeed;

        [Header("Modules")]
        [SerializeField] private Triggerable _triggerable;

        /*[Header("Projectiles")]
        [SerializeField] private float _projectileForce;
        [SerializeField] private Transform _projectilesSpawnPosition;*/

        [Header("Shield")]
        [SerializeField] private float _shieldTime;
        [SerializeField] private string _animatorShieldBoolKey = "IsShieldActivated";

        private Rigidbody _rigidbody;
        private Transform _transform;
        private Animator _animator;

        private int _healthCount;

        private bool _isAccelerating = false;
        private bool _isBraking = false;
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

        public void StartBrake() => _isBraking = true;

        public void StopBrake() => _isBraking = false;

        public void Accelerate()
        {
            _rigidbody.AddForce(_transform.forward * _accelerateForce);
        }

        public void Brake()
        {
            if (_isAccelerating) return;

            Vector3 brakeForceVector = -_rigidbody.velocity.normalized * _brakeForce * Time.fixedDeltaTime;
            if (_rigidbody.velocity.magnitude < brakeForceVector.magnitude)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            _rigidbody.AddForce(brakeForceVector);
        }


        public void Rotate()
        {
            Vector2 direction = new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical);

            if (direction.sqrMagnitude <= 0) return;

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.y;

            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.fixedDeltaTime * _rotateSpeed);

            _rigidbody.MoveRotation(Quaternion.Euler(0, newAngle, HandleTilt(direction)));
        }

        public void Rotate(Vector2 direction)
        {
            if (direction.sqrMagnitude <= 0) return;

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.y;

            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.fixedDeltaTime * _rotateSpeed);

            _rigidbody.MoveRotation(Quaternion.Euler(0, newAngle, HandleTilt(direction)));
        }

        private float HandleTilt(Vector2 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.y;
            if (currentAngle > 180) currentAngle -= 360;

            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

            float targetTilt = Mathf.Clamp(-angleDifference * 0.5f, -_tiltAmount, _tiltAmount);

            float currentTilt = transform.eulerAngles.z;
            if (currentTilt > 180) currentTilt -= 360;

            return Mathf.Lerp(currentTilt, targetTilt, Time.fixedDeltaTime * _tiltSpeed);
        }

        public void Shoot()
        {
            _triggerable.TriggerOnce();
            //_moduleMount.Modules.ForEach(module => { module.
            //ProjectilesController.instance.SpawnProjectile(_projectilesSpawnPosition.position, _transform.up * _projectileForce);
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
            if (_isBraking)
                Brake();
        }
    }
}