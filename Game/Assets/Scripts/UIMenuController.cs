using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIMenuController : MonoBehaviour
    {
        public UIObject[] Objects;
        public bool Open;
        public bool HideWhenClosed;
        public UnityEvent OnOpen;
        public UnityEvent OnClose;
        public float SliderSpeed;

        private int _currentSelection;
        private int _lastSelection;
        private int _lastOption;
        private float _value = 0;
        private bool _dis;

        private void Awake()
        {
            _currentSelection = 0;
            for (int i = 0; i < Objects.Length; i++)
            {
                int x = i;
                Objects[i].OnHover.AddListener(() => ChangeSelectionTo(x));
            }
            if (Open)
                OpenMenu();
            else
                CloseMenu();
        }

        private void Start()
        {
            UpdateSelection();
        }

        private void Update()
        {
            if (Open)
            {
                _dis = false;
                if (Objects[_currentSelection].GetType() == typeof(UISlider) && _value != 0)
                {
                    ((UISlider)Objects[_currentSelection]).ChangeValueBy(_value * Time.deltaTime * SliderSpeed);
                }
            }
            
        }

        public void Confirm(InputAction.CallbackContext context)
        {
            if (context.performed)
                Confirm();
        }

        public void Confirm()
        {
            if (!Open || _dis)
                return;
            if (Objects[_currentSelection].GetType() == typeof(UIButton))
            {
                ((UIButton)Objects[_currentSelection]).Press();
                _dis = true;
            }
        }

        public void ChangeSelectionBy(InputAction.CallbackContext context)
        {
            ChangeSelectionBy(Mathf.RoundToInt(context.ReadValue<float>()));
        }

        public void ChangeSelectionBy(int value)
        {
            if (!Open || _dis)
                return;
            if (value == _lastSelection)
                return;
            _lastSelection = value;
            _currentSelection = Mathf.Clamp(_currentSelection + value, 0, Objects.Length - 1);
            UpdateSelection();
        }

        public void ChangeSelectionTo(int id)
        {
            if (!Open || _dis)
                return;
            if (id >= Objects.Length)
                return;
            _currentSelection = id;
            UpdateSelection();
        }

        public void ChangeOptionBy(InputAction.CallbackContext context)
        {
            ChangeOptionBy(Mathf.RoundToInt(context.ReadValue<float>()));
        }

        public void ChangeOptionBy(int value)
        {
            if (!Open || _dis)
                return;
            if (value == _lastOption)
                return;
            _lastOption = value;
            if (Objects[_currentSelection].GetType() == typeof(UIList))
                ((UIList)Objects[_currentSelection]).ChangeOptionBy(value);
        }

        public void ChangeValueBy(InputAction.CallbackContext context)
        {
            ChangeValueBy(context.ReadValue<float>());
        }

        public void ChangeValueBy(float value)
        {
            if (!Open || _dis)
                return;
            _value = value;
        }

        public void UpdateSelection()
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                if (Objects[i].BackgroundImage != null)
                {
                    Objects[i].BackgroundImage.material = new Material(Global.Instance.SpriteShader);
                    Objects[i].BackgroundImage.material.SetColor("_MainColor", Global.Instance.CurrentColorPalette.MainColor);
                    Objects[i].BackgroundImage.material.SetColor("_SecondColor", Global.Instance.CurrentColorPalette.SecondColor);
                }
                if (Objects[i].Text != null)
                {
                    Objects[i].Text.fontMaterial = new Material(Objects[i].Text.fontMaterial);
                    Objects[i].Text.fontMaterial.SetColor("_MainColor", Global.Instance.CurrentColorPalette.MainColor);
                    Objects[i].Text.fontMaterial.SetColor("_SecondColor", Global.Instance.CurrentColorPalette.SecondColor);
                    Objects[i].Text.fontMaterial.SetTexture("_FontTexture", Global.Instance.FontTexture);
                }
                if (i == _currentSelection)
                {
                    if (Objects[i].BackgroundImage != null)
                    {
                        Objects[i].BackgroundImage.material.SetInt("_InvertColors", 0);
                    }
                    if (Objects[i].Text != null)
                    {
                        Objects[i].Text.fontMaterial.SetInt("_InvertColors", 1);
                    }
                }
                else
                {
                    if (Objects[i].BackgroundImage != null)
                    {
                        Objects[i].BackgroundImage.material.SetInt("_InvertColors", 1);
                    }
                    if (Objects[i].Text != null)
                    {
                        Objects[i].Text.fontMaterial.SetInt("_InvertColors", 0);
                    }
                }
            }
        }

        public void OpenMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
                OpenMenu();
        }

        public void OpenMenu()
        {
            gameObject.SetActive(true);
            OnOpen.Invoke();
            Open = true;
        }

        public void CloseMenu()
        {
            if (HideWhenClosed)
                gameObject.SetActive(false);
            _value = 0;
            OnClose.Invoke();
            Open = false;
            _dis = true;
        }

        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}
