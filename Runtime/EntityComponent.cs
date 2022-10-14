using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public abstract class EntityComponent : MonoBehaviour
    {
        protected void Awake()
        {
            GetComponent<Entity>().UpdateComponents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += GetComponent<Entity>().UpdateComponents;
#endif
        }

        protected void OnDestroy()
        {
            GetComponent<Entity>().UpdateComponents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += GetComponent<Entity>().UpdateComponents;
#endif
        }
    }
}