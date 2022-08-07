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

        public Dictionary<Type, EntityComponent> Components = new();
        public Dictionary<Type, EntityComponent> InactiveComponents = new();

#if UNITY_EDITOR
        public void DebugEditor()
        {
            m_Components = new();
            foreach (var c in Components)
                m_Components.Add(c.Value);
        }
#endif
    }

    public static class EntityExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity AddComponent<T>(this Entity entity) where T : EntityComponent
        {
            if (entity.InactiveComponents.ContainsKey(typeof(T)))
            {
                var c = entity.InactiveComponents[typeof(T)];
                c.IsActive = true;
                entity.Components.Add(typeof(T), c);
                entity.InactiveComponents.Remove(typeof(T));
            }
            else if (!entity.Components.ContainsKey(typeof(T)))
            {
                if (entity.gameObject.GetComponent<T>() == null)
                    entity.Components.Add(typeof(T), entity.gameObject.AddComponent<T>());
#if DEBUG
                else
                    Debug.LogError($"{entity.gameObject.name} already has a {typeof(T).Name} component");
#endif
            }
#if DEBUG
            else
                Debug.LogError($"EntityExt -> AddComponent<T> :: Entity: {entity.name} already has Component of Type: " + typeof(T).Name);
#endif

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity AddComponent<T>(this Entity entity, T component) where T : EntityComponent
        {
            component.IsActive = true;
            if (entity.InactiveComponents.ContainsValue(component))
            {
                entity.Components.Add(typeof(T), component);
                entity.InactiveComponents.Remove(typeof(T));
            }
            else if (!entity.Components.ContainsValue(component))
            {
                if (entity.gameObject.GetComponent<T>() == null)
                    entity.Components.Add(typeof(T), component);
#if DEBUG
                else
                    Debug.LogError($"{entity.gameObject.name} already has a {typeof(T).Name} component");
#endif
            }
#if DEBUG
            else
                Debug.LogError($"EntityExt -> AddComponent<T> :: Entity: {entity.name} already has Component of Type: " + typeof(T).Name);
#endif

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Entity> AddComponents<T>(this IEnumerable<Entity> entities) where T : EntityComponent
        {
            foreach (var e in entities) e.AddComponent<T>();
            return entities;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Entity> AddComponents<T>(this IEnumerable<Entity> entities, T component) where T : EntityComponent
        {
            foreach (var e in entities) e.AddComponent(component.DeepCopy());
            return entities;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity RemoveComponent<T>(this Entity entity) where T : EntityComponent
        {
            if (entity.Components.ContainsKey(typeof(T)))
            {
                var c = entity.Components[typeof(T)];
                c.IsActive = false;
                entity.InactiveComponents.Add(typeof(T), c);
                entity.Components.Remove(typeof(T));
            }
#if DEBUG
            else
                Debug.LogError($"EntityExt -> RemoveComponent<T> :: Entity: {entity.name} does not have Component of Type: " + typeof(T).Name);
#endif

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity RemoveComponent<T>(this Entity entity, T component) where T : EntityComponent
        {
            component.IsActive = false;
            if (entity.Components.ContainsValue(component))
            {
                entity.InactiveComponents.Add(typeof(T), entity.Components[typeof(T)]);
                entity.Components.Remove(typeof(T));
            }
#if DEBUG
            else
                Debug.LogError($"EntityExt -> RemoveComponent<T> :: Entity: {entity.name} does not have Component of Type: " + typeof(T).Name);
#endif

            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity RemoveAllComponents(this Entity entity)
        {
            entity.Components.Clear();
            entity.InactiveComponents.Clear();
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Entity> RemoveAllComponents(this IEnumerable<Entity> entities)
        {
            foreach (var e in entities) e.RemoveAllComponents();
            return entities;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Entity> RemoveComponents<T>(this IEnumerable<Entity> entities) where T : EntityComponent
        {
            foreach (var e in entities) e.RemoveComponent<T>();
            return entities;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Component<T>(this Entity entity) where T : EntityComponent
        {
            if (entity.Components.ContainsKey(typeof(T)))
                return (T)entity.Components[typeof(T)];

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> Components<T>(this IEnumerable<Entity> entity, ref List<T> listOut, bool includeInactive) where T : EntityComponent
        {
            listOut.Clear();
            foreach (var e in entity)
                if (e.Components.ContainsKey(typeof(T)) && (e.gameObject.activeInHierarchy || includeInactive))
                {
                    var c = e.Component<T>();
                    if (c != null) listOut.Add(c);
                }

            return listOut;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponent<T>(this Entity entity) where T : EntityComponent
        {
            return entity.Components.ContainsKey(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasComponent<T>(this Entity entity, T component) where T : EntityComponent
        {
            return entity.Components.ContainsValue(component);
        }
    }
}
