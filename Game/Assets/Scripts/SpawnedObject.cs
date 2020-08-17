using UnityEngine;

namespace Assets.Scripts
{
    public class SpawnedObject : MonoBehaviour
    {
        [HideInInspector] public int ID;
        [HideInInspector] public int ObjectID;
        [HideInInspector] public Spawner Spawner;

        public void Return()
        {
            if (Spawner != null)
                Spawner.Return(ID, ObjectID);
        }
    }
}
