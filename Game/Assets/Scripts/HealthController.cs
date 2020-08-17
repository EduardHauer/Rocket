using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class HealthController : MonoBehaviour
    {
        public int MaxHP;
        public int CurrentHP;
        public UnityEvent<int> OnDamage;
        public UnityEvent<GameObject> OnDeath;
        public string DamagerTag;

        private Renderer _renderer;
        private Coroutine _coroutine;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            CurrentHP = MaxHP;
        }

        private void OnEnable()
        {
            CurrentHP = MaxHP;
        }

        public void GetDamage(int damage)
        {
            CurrentHP -= damage;
            OnDamage.Invoke(damage);
            if (CurrentHP <= 0)
            {
                if (_renderer != null)
                    _renderer.material.SetFloat("_Flash", 0);
                CurrentHP = 0;
                OnDeath.Invoke(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == DamagerTag)
            {
                if (collision.gameObject.GetComponent<Damager>() != null)
                {
                    GetDamage(collision.gameObject.GetComponent<Damager>().Damage);
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
