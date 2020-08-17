using UnityEngine;

namespace Assets.Scripts
{
    public class PixelPerfect : MonoBehaviour
    {
        public Transform PixelPerfectTransform;
        public Vector2Int Grid;

        private void Update()
        {
            PixelPerfectTransform.position = new Vector3(Mathf.Round(transform.position.x * Grid.x) / Grid.x, Mathf.Round(transform.position.y * Grid.y) / Grid.y, transform.position.z);
        }
    }
}
