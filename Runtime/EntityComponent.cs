using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    [ExecuteInEditMode]
    public abstract class EntityComponent : MonoBehaviour
    {
        public Entity Entity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> ComponentsOfType<T>(ref List<T> listOut) where T : EntityComponent => Entity.ComponentsOfType<T>(ref listOut, true);

        protected void Awake()
        {
            Entity = GetComponent<Entity>();
            Entity.UpdateComponents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += Entity.UpdateComponents;
#endif
        }

        protected void OnDestroy()
        {
            Entity.UpdateComponents();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += Entity.UpdateComponents;
#endif
        }
    }
}