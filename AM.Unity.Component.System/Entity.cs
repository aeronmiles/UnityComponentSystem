using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;

namespace AM.Unity.Component.System
{
    public class Entity : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] List<EntityComponent> m_Components;
        public Dictionary<Type, HashSet<EntityComponent>> Components = new();

#if UNITY_EDITOR
        public void Debug()
        {
            m_Components = new();
            foreach (var c in Components)
                m_Components.AddRange(c.Value);
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
            if (entity.Components.ContainsKey(typeof(T)))
            {
                var components = entity.Components[typeof(T)];
                foreach (var c in components)
                    if (includeInactive || c.gameObject.activeInHierarchy) listOut.Add((T)c);
            }

            return listOut;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T>(this Entity entity) where T : EntityComponent
        {
            return entity.Components.ContainsKey(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent
        {
            return entity.Components.ContainsKey(typeof(T0)) && entity.Components.ContainsKey(typeof(T1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1, T2>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent
        {
            return entity.Components.ContainsKey(typeof(T0)) && entity.Components.ContainsKey(typeof(T1)) && entity.Components.ContainsKey(typeof(T2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponents<T0, T1, T2, T3>(this Entity entity) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent  where T3 : EntityComponent
        {
            return entity.Components.ContainsKey(typeof(T0)) && entity.Components.ContainsKey(typeof(T1)) && entity.Components.ContainsKey(typeof(T2)) && entity.Components.ContainsKey(typeof(T3));
        }
    }
}
