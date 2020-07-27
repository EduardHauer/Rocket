using UnityEngine;

namespace Assets.Scripts
{
    public class StatController : MonoBehaviour
    {
        public static StatController Instance;

        public int Damage;
        public int Speed;

        public Material Background;

        private void Awake()
        {
            Instance = this;
            Background.SetVector("_Speed", new Vector4(Speed / 4f, 0, 0, 0));
        }

        public void AddDamage()
        {
            Damage++;
        }

        public void AddSpeed()
        {
            Speed++;
            Background.SetVector("_Speed", new Vector4(Speed / 4f, 0, 0, 0));
        }
    }
}
