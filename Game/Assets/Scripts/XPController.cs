using UnityEngine;

namespace Assets.Scripts
{
    public class XPController : MonoBehaviour
    {
        public void GiveXP(int xp)
        {
            XPSystem.Instance.Add(xp);
        }

        public void GiveXP(int min, int max)
        {
            GiveXP(Random.Range(min, max));
        }
        public void GiveXP(Vector2Int range)
        {
            GiveXP(Random.Range(range.x, range.y));
        }
    }
}
