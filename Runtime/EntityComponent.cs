using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityComponent : MonoBehaviour
    {
        private void Awake() => EntityManager.I(gameObject.scene).Add(GetComponent<Entity>());

        private void OnDestroy() => EntityManager.I(gameObject.scene).Remove(GetComponent<Entity>());
    }
}