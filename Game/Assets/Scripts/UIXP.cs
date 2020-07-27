using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIXP : MonoBehaviour
    {
        public static UIXP Instance;

        public Slider CurrentXP;
        public Slider GoingToXP;
        public UIText LevelText;
        public float FillSpeed;
        public Menu StatMenu;

        private ObjectPooler _objectPooler;
        private Spawner _spawner;
        [SerializeField] private float _currentXP;
        private float _progressBarXP;
        [SerializeField] private float _maxXP = 100;
        private int _currentLevel;

        private void Awake()
        {
            Instance = this;
            CurrentXP.value = _currentXP / _maxXP * CurrentXP.maxValue;
            GoingToXP.value = _currentXP / _maxXP * GoingToXP.maxValue;
            _progressBarXP = _currentXP;
            LevelText.SetText(_currentLevel.ToString());
        }

        private void Start()
        {
            _objectPooler = ObjectPooler.Instance;
            //_spawner = Spawner.Instance;
        }

        private void Update()
        {
            if (_progressBarXP + Time.deltaTime * FillSpeed / 2 < _currentXP)
                _progressBarXP += Time.deltaTime * FillSpeed;
            //else if (_progressBarXP - Time.deltaTime * FillSpeed / 2 > _currentXP)
            //    _progressBarXP -= Time.deltaTime * FillSpeed;
            else
            {
                _progressBarXP = _currentXP;
                //_currentXP = Random.Range(0, _maxXP);
            }
            if (_progressBarXP > _maxXP)
            {
                _progressBarXP = 0;
                _currentXP = _currentXP - _maxXP;
                _currentLevel++;
                _maxXP *= 1.5f;
                FillSpeed *= 1.5f;
                LevelText.SetText(_currentLevel.ToString());
                //InputController.Instance.Vibrate(1, 1, .3f);
                foreach (GameObject obj in _objectPooler.OutOfPool["Meteorite"].ToArray())
                    _objectPooler.ReturnToPool("Meteorite", obj);
                _objectPooler.AddToPool("Meteorite", 1);
                if (_spawner == null)
                    _spawner = Spawner.Instance;
                _spawner.SpawnAll(0);
                //PlayerController.Instance.HealthController.MaxHP++;
                //PlayerController.Instance.HealthController.CurrentHP = PlayerController.Instance.HealthController.MaxHP;
                //PlayerController.Instance.HealthUpdate();
                StatMenu.OpenMenu(true);
            }
            CurrentXP.value = _progressBarXP / _maxXP * CurrentXP.maxValue;
            GoingToXP.value = _currentXP / _maxXP * GoingToXP.maxValue;
        }

        public void Add(float xp)
        {
            _currentXP += xp;
        }
    }
}