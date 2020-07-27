using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIGameOver : MonoBehaviour
    {
        public Material TransparentMaterial;
        public float Speed;
        public UnityEvent OnAnimationEnd;

        private float _alpha;
        private bool _gameOver;
        private bool _canRestart;

        private void Awake()
        {
            _canRestart = false; 
            _alpha = 0;
            _gameOver = false;
            TransparentMaterial.SetFloat("_Alpha", _alpha);
        }

        private void Update()
        {
            if (!_gameOver)
                return;
            if (_alpha < 1)
                _alpha += Time.deltaTime * Speed;
            else
            {
                _alpha = 1;
                OnAnimationEnd.Invoke();
                _canRestart = true;
            }
            TransparentMaterial.SetFloat("_Alpha", _alpha);
        }

        public void GameOver()
        {
            _gameOver = true;
        }

        public void Restart()
        {
            Debug.Log("+++");
            if (_canRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}