using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : ShootingEntity
    {
        public float Step = 1;
        public Vector2 Clamp;
        public Transform BulletSpawner;

        private ObjectPooler _objectPooler;

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;
        }

        public override void Move(float direction)
        {
            if (transform.position.y + direction * Step < Clamp.y + direction * Step / 2 && transform.position.y + direction * Step > Clamp.x + direction * Step / 2)
                transform.position += direction * Vector3.up * Step;
        }

        public override void Shoot()
        {
            Debug.Log("PEW");
            _objectPooler.SpawnFromPool("Bullets", BulletSpawner.position, BulletSpawner.rotation);
        }
    }
}
