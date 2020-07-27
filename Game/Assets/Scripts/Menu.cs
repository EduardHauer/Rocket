using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Menu : MonoBehaviour
    {
        //public static Menu Instance;

        public Selection[] Selections;
        public bool Open;
        [ColorUsage(true, true)]
        public Color White;
        [ColorUsage(true, true)]
        public Color Black;
        public Shader HDRColorShader;
        public Shader HDRFontShader;
        public Texture FontTexture;
        public UnityEvent OnConfirm;
        public InputActionMap InputMap;

        private int _currentSelection;
        private int _lastDirection = 0;
        private InputAction _confirm;
        private InputAction _openMenu;
        private InputAction _select;
        private InputAction _exit;

        [Serializable]
        public struct Selection
        {
            public Image background;
            public TextMeshProUGUI text;
            public UnityEvent onConfirm;
        }

        private void Awake()
        {
            _confirm = InputMap.FindAction("Confirm");
            _openMenu = InputMap.FindAction("OpenMenu");
            _select = InputMap.FindAction("Select");
            _exit = InputMap.FindAction("Exit");
            _confirm.performed += _ => Confirm();
            _openMenu.performed += _ => OpenMenu(true);
            _select.performed += _ => Select(Mathf.RoundToInt(_.ReadValue<float>()));
            _select.canceled += _ => Select(0);
            _exit.performed += _ => Exit();
            _currentSelection = 0;
            //Instance = this;
            if (Open)
            {
                Time.timeScale = 0;
                gameObject.SetActive(true);
            }
            else
                gameObject.SetActive(false);

            for (int i = 0; i < Selections.Length; i++)
            {
                if (Selections[i].background != null)
                    Selections[i].background.material = new Material(HDRColorShader);
                if (Selections[i].text != null)
                {
                    Selections[i].text.fontMaterial = new Material(HDRFontShader);
                    Selections[i].text.fontMaterial.SetTexture("_FontTexture", FontTexture);
                }
                if (i == _currentSelection)
                {
                    if (Selections[i].background != null)
                    {
                        Selections[i].background.material.SetColor("_Color", White);
                    }
                    if (Selections[i].text != null)
                    {
                        Selections[i].text.fontMaterial.SetColor("_Color", Black);
                    }
                }
                else
                {
                    if (Selections[i].background != null)
                    {
                        Selections[i].background.material.SetColor("_Color", Black);
                    }
                    if (Selections[i].text != null)
                    {
                        Selections[i].text.fontMaterial.SetColor("_Color", White);
                    }
                }
            }

        }

        private void OnEnable()
        {
            InputMap.Enable();
        }

        public void Confirm()
        {
            if (Open)
            {
                if (_currentSelection < Selections.Length)
                    Selections[_currentSelection].onConfirm.Invoke();
                OnConfirm.Invoke();
            }
        }

        public void OpenMenu(bool open)
        {
            if (open && !Open)
            {
                Time.timeScale = 0;
                gameObject.SetActive(true);
                Open = true;
            }
            else if(!open && Open)
            {
                Time.timeScale = 1;
                gameObject.SetActive(false);
                Open = false;
            }
        }

        public void Select(int direction)
        {
            if (!Open)
                return;
            Debug.Log(direction);
            if (_lastDirection == direction)
                return;
            _lastDirection = direction;
            if (_currentSelection + direction < Selections.Length && _currentSelection + direction >= 0)
                _currentSelection += direction;
            for (int i = 0; i < Selections.Length; i++)
            {
                if (i == _currentSelection)
                {
                    if (Selections[i].background != null)
                    {
                        Selections[i].background.material.SetColor("_Color", White);
                    }
                    if (Selections[i].text != null)
                    {
                        Selections[i].text.fontMaterial.SetColor("_Color", Black);
                    }
                }
                else
                {
                    if (Selections[i].background != null)
                    {
                        Selections[i].background.material.SetColor("_Color", Black);
                    }
                    if (Selections[i].text != null)
                    {
                        Selections[i].text.fontMaterial.SetColor("_Color", White);
                    }
                }
            }
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Exit()
        {
            if (Open)
                Application.Quit();
        }
    }
}