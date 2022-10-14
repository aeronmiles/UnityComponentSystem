using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public abstract class EntityComponent : MonoBehaviour
    {
        protected void Awake()
        {
            GetComponent<Entity>().AddComponent(this);
        }

        protected void OnDestroy()
        {
            GetComponent<Entity>().RemoveComponent(this);
        }
    }
}