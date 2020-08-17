using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIObject : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnHover;
        public Image BackgroundImage;
        public TextMeshProUGUI Text;
    }
}
