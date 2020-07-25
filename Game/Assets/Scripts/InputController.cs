using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        public InputActionAsset Asset;
        public ShootingEntity Player;

        private InputActionMap _player;
        private InputAction _movement;
        private InputAction _shooting;

        private void Awake()
        {
            _player = Asset.FindActionMap("Player");
            _movement = _player.FindAction("Movement");
            _shooting = _player.FindAction("Shooting");

            _movement.performed += _ => Player.Move(_.ReadValue<float>());
            _shooting.performed += _ => Player.Shoot();
        }

        private void OnEnable()
        {
            Asset.Enable();
        }

        private void OnDisable()
        {
            Asset.Disable();
        }
    }
}