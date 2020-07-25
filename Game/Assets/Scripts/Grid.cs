using UnityEngine;

namespace Assets.Scripts
{
    public class Grid : MonoBehaviour
    {
        public Vector2 Size;
        public GameObject Tile;
        public Transform Parent;

        private void Awake()
        {
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Instantiate(Tile, new Vector3(x + 0.5f, y + 0.5f, 1) - (Vector3)Size / 2, Quaternion.Euler(Vector3.zero), Parent).GetComponent<SpriteRenderer>().color = new Color(x*1f/Size.x, y*1f/Size.y, 0);
        }
    }
}