using UnityEngine;

namespace Assets.Scripts
{
    public class Meteorite : Entity
    {
        public float Speed;
        public Vector2 OutOfBounds;
        public Vector2 XPRange;

        private StatController _statController;
        private ObjectPooler _objectPooler;
        private UIXP _UIXP;
        private Spawner _spawner;
        private Vector3 _position;

        private void Start()
        {
            _position = transform.position;
            _statController = StatController.Instance;
            _objectPooler = ObjectPooler.Instance;
            _UIXP = UIXP.Instance;
            _spawner = Spawner.Instance;
            Speed = _statController.Speed;
        }

        private void OnEnable()
        {
            _position = transform.position;
            if (_statController != null)
                Speed = _statController.Speed;
        }

        private void OnDisable()
        {
            _position = transform.position;
        }

        private void Update()
        {
            Move(-1 * Speed);
            if (_position.x > OutOfBounds.y || _position.x < OutOfBounds.x)
            {
                _objectPooler.ReturnToPool("Meteorite", gameObject);
                Spawn();
            }
            transform.position = new Vector3(Mathf.Round(_position.x * 16) / 16, Mathf.Round(_position.y * 16) / 16, Mathf.Round(_position.z * 16) / 16);
        }

        public override void Move(float direction)
        {
            _position += direction * Time.deltaTime * Vector3.right;
        }

        public void GiveXP()
        {
            _UIXP.Add(Random.Range(XPRange.x, XPRange.y));
        }

        public void Spawn()
        {
            _spawner.Spawn(0);
        }
    }
}
