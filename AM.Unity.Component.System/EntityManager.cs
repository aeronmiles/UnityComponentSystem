using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AM.Unity.Component.System
{
    public class EntityManager : MonoSingletonScene<EntityManager>
    {
        [Header("Debug")]
#pragma warning disable IDE0052
        [SerializeField] List<Entity> m_EntityList;
#pragma warning restore IDE0052
        static HashSet<Entity> m_Entitites = new();

        private void OnEnable()
        {
            m_Entitites.Clear();
            var entities = GetComponentsInChildren<Entity>(true);
            foreach (var e in entities) Add(e);
        }

        public static void Instantiate(Entity entity, int count = 1)
        {
            var parent = entity.transform.parent;
            for (int i = 0; i < count; i++)
                Add(Instantiate(entity, parent));
        }

        public static void Destroy(Entity entity)
        {
            Remove(entity);
            Destroy(entity.gameObject);
        }

        public static void Add(Entity entity)
        {
            if (m_Entitites.Contains(entity)) return;

            var components = entity.GetComponents<EntityComponent>();
            entity.Components.Clear();
            foreach (var c in components) entity.AddComponent(c);
            m_Entitites.Add(entity);

#if UNITY_EDITOR
            entity.DebugEditor();
            I.m_EntityList = m_Entitites.ToList();
#endif
        }

        public static void Remove(Entity entity)
        {
            if (!m_Entitites.Contains(entity)) return;
            m_Entitites.Remove(entity);

#if UNITY_EDITOR
            I.m_EntityList = m_Entitites.ToList();
#endif
        }

        public static List<T> GetComponents<T>(ref List<T> listOut, bool includeInactive = true) where T : EntityComponent
        {
            listOut.Clear();
            var itemsOfT = MemPool.Get<List<T>>();
            m_Entitites.Components(ref itemsOfT, includeInactive);
            listOut.AddRange(itemsOfT);

            itemsOfT.Clear();
            itemsOfT.FreeTo_MemPool();

            return listOut;
        }

        public static List<Entity> With<T>(ref List<Entity> listOut, bool includeInactive = true) where T : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponent<T>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }
    }
}