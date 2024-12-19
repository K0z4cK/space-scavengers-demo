using UnityEngine;

namespace SpaceScavengers
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] InputActions _actions;

        private void Awake()
        {
            _actions = new InputActions();
            _actions.Enable();
        }

        private void OnEnable()
        {
            _actions.Player.Shoot.performed += PerformShoot;
            _actions.Player.Acelerate.started += AcelerateStarted;
            _actions.Player.Acelerate.canceled += AcelerateCanceled;
        }

        private void AcelerateStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _player.StartAccelerate();

        }
        private void AcelerateCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _player.StopAccelerate();
        }

        private void PerformShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _player.Shoot();
        }

        private void FixedUpdate()
        {
            _player.Rotate(_actions.Player.Rotate.ReadValue<Vector2>());
        }
    }
}
