using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityComponent : MonoBehaviour
    {
        private void Awake()
        {
            EntityManager.Add(GetComponent<Entity>());
        }

        private void OnDestroy()
        {
            EntityManager.Remove(GetComponent<Entity>());
        }
    }
}