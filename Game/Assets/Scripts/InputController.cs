using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        private Coroutine _vibration;

        private void Awake()
        {
            Instance = this;
        }

        public void Vibrate(float low, float high, float time)
        {
            if (_vibration != null)
                StopCoroutine(_vibration);
            if (Gamepad.all.Count > 0)
                _vibration = StartCoroutine(Vibr(low, high, time));
        }

        private IEnumerator Vibr(float low, float high, float time)
        {
            Gamepad.current.SetMotorSpeeds(low, high);
            yield return new WaitForSeconds(time);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}