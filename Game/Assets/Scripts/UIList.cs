using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIList : UIObject
    {
        public UnityEvent<int> OnOptionChange;
        public string[] Options;
        public TextMeshProUGUI OptionText;
        public int CurrentOption;

        private void Start()
        {
            ChangeOptionTo(CurrentOption % Options.Length);
        }

        public void ChangeOptionTo(int id)
        {
            if (id >= Options.Length)
                return;
            OnOptionChange.Invoke(id);
            OptionText.text = Options[id];
            CurrentOption = id;
        }

        public void ChangeOptionBy(int value)
        {
            ChangeOptionTo((Options.Length + (CurrentOption + value) % Options.Length) % Options.Length);
        }

        public void Hover()
        {
            OnHover.Invoke();
        }
    }
}