using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerController : Entity
    {
        //public static PlayerController Instance;

        public float Speed = 1;
        public Animator Animator;
        public Vector2 Clamp;
        public Transform BulletSpawner;
        public HealthController HealthController;
        public Renderer HealthSprite;
        public UIMenuController PauseMenu;
        public UIMenuController StatMenu;

        private float _direction;
        private Vector2 _position;

        private void Awake()
        {
            //Instance = this;
            HealthUpdate();
            _position = transform.position;
        }

        private void Update()
        {
            _position = new Vector2(_position.x, Mathf.Clamp(_position.y + _direction * Speed * Time.deltaTime, Clamp.x, Clamp.y));
            transform.position = new Vector3(Mathf.Round(_position.x * 16) / 16, Mathf.Round(_position.y * 16) / 16, transform.position.z);
        }

        public void Move(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<float>());
        }

        public override void Move(float direction)
        {
            _direction = direction;
            Animator.SetInteger("Direction", Mathf.CeilToInt(direction));
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if (context.performed)
                Shoot();
        }

        public void Shoot()
        {
            
        }

        public void HealthUpdate()
        {
            HealthSprite.material.SetFloat("_MaxHP", HealthController.MaxHP);
            HealthSprite.material.SetFloat("_CurrentHP", HealthController.CurrentHP);
        }
    }
}
