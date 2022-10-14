using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;

namespace AM.Unity.Component.System
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class Entity : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] List<EntityComponent> m_ComponentList;

        public Dictionary<Type, EntityComponent> Components = new();

        internal void AddComponent(EntityComponent component)
        {
            Components.Add(component.GetType(), component);
        }

        internal void RemoveComponent(EntityComponent component)
        {
            Type t = component.GetType();
            if (Components.ContainsKey(t))
                if (Components[t] == component)
                    Components.Remove(t);
        }

#if UNITY_EDITOR
        private void Awake() => EntityManager.I(gameObject.scene).Add(this);

        private void OnDestroy() => EntityManager.I(gameObject.scene).Remove(this);

        public void Editor_Debug()
        {
            m_ComponentList.Clear();
            m_ComponentList.AddRange(Components.Values);
        }

        private void Update()
        {
            Awake();
            Editor_Debug();
        }
#endif
    }

    public static class EntityExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ComponentsOfType<T>(this Entity entity, ref List<T> listOut, bool includeInactive) where T : EntityComponent
        {
            listOut.Clear();
            if (entity.Components == null) return listOut;
            foreach (var c in entity.Components)
            {
                if (c.Value as T != null)
                    if (includeInactive || c.Value.gameObject.activeInHierarchy)
                        listOut.Add((T)c.Value);
            }

            return listOut;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ComponentsOfType<T>(this IEnumerable<Entity> entity, ref List<T> listOut, bool includeInactive) where T : EntityComponent
        {
            listOut.Clear();
            var tempList = MemPool.Get<List<T>>();

            foreach (var e in entity)
                listOut.AddRange(e.ComponentsOfType<T>(ref tempList, includeInactive));

            tempList.Clear();
            MemPool.Free(tempList);

            return listOut;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponent<T>(this Entity entity) where T : EntityComponent
        {
            var tempList = MemPool.Get<List<T>>();
            bool hasComponent = entity.ComponentsOfType(ref tempList, true).Count > 0;
            tempList.Clear();
            MemPool.Free(tempList);

            return hasComponent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent
        {
            return entity.HasComponent<T0>() && entity.HasComponent<T1>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1, T2>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent
        {
            return entity.HasComponent<T0>() && entity.HasComponent<T1>() && entity.HasComponent<T2>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1, T2, T3>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent where T3 : EntityComponent
        {
            return entity.HasComponent<T0>() && entity.HasComponent<T1>() && entity.HasComponent<T2>() && entity.HasComponent<T3>();
        }
    }
}
