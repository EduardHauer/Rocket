using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
            //Menu.AnyOpen = true;
        }

        public void Restart(InputAction.CallbackContext context)
        {
            if (context.performed)
                Restart();
        }

        public void Restart()
        {
            if (_canRestart)
            {
                //Menu.AnyOpen = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}