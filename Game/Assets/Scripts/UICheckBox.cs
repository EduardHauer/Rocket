using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class UICheckBox : MonoBehaviour
    {
        public GameObject CheckBox;
        public bool Boolean;
        public UnityEvent<bool> OnBooleanChange;

        public void SetBoolean(bool boolean)
        {
            Boolean = boolean;
            CheckBox.SetActive(Boolean);
            OnBooleanChange.Invoke(boolean);
        }

        public void SwitchBoolean()
        {
            SetBoolean(!Boolean);
        }
    }
}
