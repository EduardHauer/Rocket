using UnityEngine;

namespace Assets.Scripts
{
    public class BulletController : Entity
    {
        public float Speed;
        public float Direction;
        public Vector2 OutOfBounds;

        private Vector3 _position;
        private ObjectPooler _objectPooler;

        private void Start()
        {
            _position = transform.position;
            _objectPooler = ObjectPooler.Instance;
        }

        private void OnEnable()
        {
            Debug.Log("ENABLED");
            _position = transform.position;
        }

        private void OnDisable()
        {
            Debug.Log("DISABLED");
            _position = transform.position;
        }

        private void Update()
        {
            Move(Direction);
            if (_position.x > OutOfBounds.y || _position.x < OutOfBounds.x)
                _objectPooler.ReturnToPool("Bullets", gameObject);
            transform.position = new Vector3(Mathf.Round(_position.x * 16) / 16, Mathf.Round(_position.y * 16) / 16, Mathf.Round(_position.z * 16) / 16);
        }

        public override void Move(float direction)
        {
            _position += direction * Time.deltaTime * Speed * Vector3.right;
        }
    }
}
