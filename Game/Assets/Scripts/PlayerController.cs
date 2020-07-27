using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : ShootingEntity
    {
        //public static PlayerController Instance;

        public float Step = 1;
        public Vector2 Clamp;
        public Transform BulletSpawner;
        public HealthController HealthController;
        public Renderer HealthSprite;
        public Menu PauseMenu;
        public Menu StatMenu;

        private ObjectPooler _objectPooler;
        private InputController _inputController;
        private StatController _statController;
        private float _lastDirection;
        private bool _lastPerformerd;
        private float _t;

        private void Awake()
        {
            //Instance = this;
            HealthUpdate();
        }

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;
            _inputController = InputController.Instance;
            _statController = StatController.Instance;
        }

        private void Update()
        {
            if (_t > 0)
                _t -= Time.deltaTime;
            else
                _t = 0;
            if (PauseMenu.Open || StatMenu.Open)
                _t = .1f;
        }

        public override void Move(float direction)
        {
            if (_t > 0)
                return;
            if (direction < -.5f)
                direction = -1;
            else if (direction > .5f)
                direction = 1;
            else
                direction = 0;
            if (direction == _lastDirection)
                return;
            if (transform.position.y + direction * Step < Clamp.y + direction * Step / 2 && transform.position.y + direction * Step > Clamp.x + direction * Step / 2)
                transform.position += direction * Vector3.up * Step;
            _lastDirection = direction;
        }

        public override void Shoot(bool performed)
        {
            if (_t > 0)
                return;
            if (performed == _lastPerformerd)
                return;
            _lastPerformerd = performed;
            if (!performed)
                return;
            GameObject obj = _objectPooler.SpawnFromPool("Bullets", BulletSpawner.position, BulletSpawner.rotation);
            if (obj != null) 
            {
                _inputController.Vibrate(0, .75f, .2f);
                if (obj.GetComponent<Damager>() != null)
                    obj.GetComponent<Damager>().Damage = _statController.Damage;
            }
        }

        public void HealthUpdate()
        {
            HealthSprite.material.SetFloat("_MaxHP", HealthController.MaxHP);
            HealthSprite.material.SetFloat("_CurrentHP", HealthController.CurrentHP);
        }
    }
}
