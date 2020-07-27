using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class UIText : MonoBehaviour
    {
        public TextMeshProUGUI[] texts;
        [TextArea] public string Text;

        private void Awake()
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = Text;
            }
        }

        public void SetText(string outputText)
        {
            Text = outputText;
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = Text;
            }
        }
    }
}