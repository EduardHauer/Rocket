using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class UICursor : MonoBehaviour
    {
        private void Update()
        {
            //Camera.ScreenToWorldPoint(new Vector3(Mouse.current.position.x, Mouse.current.position.x));
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(Mathf.Round(pos.x * 16) / 16, Mathf.Round(pos.y * 16) / 16, transform.position.z);
        }
    }
}
