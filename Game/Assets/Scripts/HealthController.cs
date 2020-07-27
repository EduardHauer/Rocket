using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class HealthController : MonoBehaviour
    {
        public int MaxHP;
        public int CurrentHP;
        public UnityEvent OnDamage;
        public UnityEvent<GameObject> OnDeath;
        public string PoolTag;
        public string[] DamagerTag;

        private ObjectPooler _objectPooler;
        private Renderer _renderer;
        private Coroutine _coroutine;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            CurrentHP = MaxHP;
        }

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;
        }

        private void OnEnable()
        {
            CurrentHP = MaxHP;
        }

        public void GetDamage(int damage)
        {
            CurrentHP -= damage;
            OnDamage.Invoke();
            if (CurrentHP <= 0)
            {
                if (_renderer != null)
                    _renderer.material.SetFloat("_Flash", 0);
                CurrentHP = 0;
                if (PoolTag != string.Empty)
                    _objectPooler.ReturnToPool(PoolTag, gameObject);
                OnDeath.Invoke(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in DamagerTag)
            {
                if (collision.gameObject.tag == tag)
                {
                    if (collision.gameObject.GetComponent<Damager>() != null)
                    {
                        GetDamage(collision.gameObject.GetComponent<Damager>().Damage);
                    }
                    //else
                    break;
                }
            }
        }

        public void Flash(float time)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            if (_renderer != null)
                _coroutine = StartCoroutine(Flashing(time));
        }

        private IEnumerator Flashing(float time)
        {
            _renderer.material.SetFloat("_Flash", 1);
            yield return new WaitForSeconds(time);
            _renderer.material.SetFloat("_Flash", 0);
        }

        public void IncreaseHealth(int amount)
        {
            MaxHP += amount;
        }

        public void SetMax()
        {
            CurrentHP = MaxHP;
        }
    }
}
