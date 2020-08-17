using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class XPSystem : MonoBehaviour
    {
        public static XPSystem Instance;

        public UnityEvent<int> OnLevelUp;
        public UnityEvent<string> OnLevelUpText;
        public Slider CurrentXP;
        public Slider GoingToXP;
        public float FillSpeed;
        public float XPToLevelUp = 100;
        public float LevelMultiplier;

        private float _currentXP;
        private float _progressBarXP;
        private int _currentLevel;

        private void Awake()
        {
            Instance = this;
            CurrentXP.value = _currentXP / XPToLevelUp * CurrentXP.maxValue;
            GoingToXP.value = _currentXP / XPToLevelUp * GoingToXP.maxValue;
            _progressBarXP = _currentXP;
        }

        private void Update()
        {
            if (_progressBarXP + Time.deltaTime * FillSpeed / 2 < _currentXP)
                _progressBarXP += Time.deltaTime * FillSpeed;
            else
            {
                _progressBarXP = _currentXP;
            }
            if (_progressBarXP > XPToLevelUp)
            {
                _progressBarXP = 0;
                _currentXP = _currentXP - XPToLevelUp;
                _currentLevel++;
                XPToLevelUp *= LevelMultiplier;
                FillSpeed *= LevelMultiplier;
                OnLevelUp.Invoke(_currentLevel);
                OnLevelUpText.Invoke(_currentLevel.ToString());
            }
            CurrentXP.value = _progressBarXP / XPToLevelUp * CurrentXP.maxValue;
            GoingToXP.value = _currentXP / XPToLevelUp * GoingToXP.maxValue;
        }

        public void Add(float xp)
        {
            _currentXP += xp;
        }
    }
}