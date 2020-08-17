using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class UIButton : UIObject
    {
        public UnityEvent OnPress;

        public void Press()
        {
            OnPress.Invoke();
        }

        public void Hover()
        {
            OnHover.Invoke();
        }
    }
}