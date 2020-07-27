using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        public InputActionMap PlayerMap;
        public InputActionMap MenuMap;
        public ShootingEntity Player;
        //public Menu[] Menus;
        public UIGameOver GameOverMenu;

        //private InputActionMap _player;
        private InputAction _movement;
        private InputAction _shooting;
        //private InputActionMap _menu;
        //private InputAction _confirm;
        //private InputAction _pause;
        //private InputAction _select;
        private InputAction _restart;

        private Coroutine _vibration;

        private void Awake()
        {
            //Debug.Log(Instance == null);
            Instance = this;
            //if (Asset != null)
            //{
                //_player = Asset.FindActionMap("Player");
                _movement = PlayerMap.FindAction("Movement");
                _shooting = PlayerMap.FindAction("Shooting");
                //_menu = Asset.FindActionMap("Menu");
                //_confirm = MenuMap.FindAction("Confirm");
                //_pause = MenuMap.FindAction("Pause");
                //_select = MenuMap.FindAction("Select");
                _restart = MenuMap.FindAction("Restart");
            //}
            //else
            //    Debug.Log("Asset");

            if (Player != null)
            {
                _movement.performed += _ => Player.Move(_.ReadValue<float>());
                _movement.canceled += _ => Player.Move(0);
                _shooting.performed += _ => Player.Shoot(true);
                _shooting.canceled += _ => Player.Shoot(false);
            }
            else
                Debug.Log("Player");

            //if (Menus.Length > 0)
            //{
            //    foreach (Menu menu in Menus)
            //    {
            //        _confirm.performed += _ => menu.Confirm();
            //        _pause.performed += _ => menu.PauseGame(true);
            //        _select.performed += _ => menu.Select(Mathf.RoundToInt(_.ReadValue<float>()));
            //        _select.canceled += _ => menu.Select(Mathf.RoundToInt(0));
            //    }
            //}
            //else
            //    Debug.Log("Menus");

            if (GameOverMenu != null)
                _restart.performed += _ => GameOverMenu.Restart();
            else
                Debug.Log("GameOverMenu");
        }

        private void OnEnable()
        {
            PlayerMap.Enable();
            MenuMap.Enable();
        }

        private void OnDisable()
        {
            PlayerMap.Disable();
            MenuMap.Disable();
        }

        public void Vibrate(float low, float high, float time)
        {
            if (_vibration != null)
                StopCoroutine(_vibration);
            if (Gamepad.all.Count > 0)
                _vibration = StartCoroutine(Vibr(low, high, time));
        }

        private IEnumerator Vibr(float low, float high, float time)
        {
            Gamepad.current.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(time);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

        /*public void Clear()
        {
            _player = null;
            _movement = null;
            _shooting = null;
            _menu = null;
            _confirm = null;
            _pause = null;
            _select = null;
            _restart = null;
            Asset = null;
            Player = null;
            Menus = new Menu[0];
            GameOverMenu = null;
        }*/
    }
}