using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UISlider : UIObject
    {
        public UnityEvent<float> OnValueChange;
        public Vector2 Range;
        public Slider Slider;
        public float CurrentValue;

        private void Start()
        {
            ChangeValueTo(CurrentValue);
        }

        public void ChangeValueTo(float value)
        {
            CurrentValue = Mathf.Clamp(value, Range.x, Range.y);
            OnValueChange.Invoke(CurrentValue);
            Slider.value = (CurrentValue - Range.x) / (Range.y - Range.x) * (Slider.maxValue - Slider.minValue) + Slider.minValue;
        }

        public void ChangeValueBySlider(float value)
        {
            ChangeValueTo((value - Slider.minValue) / (Slider.maxValue - Slider.minValue) * (Range.y - Range.x) + Range.x);
        }

        public void ChangeValueBy(float value)
        {
            ChangeValueTo(CurrentValue + value);
        }

        public void Hover()
        {
            OnHover.Invoke();
        }
    }
}
