using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AM.Unity.Component.System
{
    public class EntityManager : MonoSingletonScene<EntityManager>
    {

        [Header("Debug")]
        [SerializeField] List<Entity> m_EntityList;
        HashSet<Entity> m_Entitites = new();

        private void OnEnable()
        {
            m_Entitites.Clear();
            var entities = GetComponentsInChildren<Entity>(true);
            foreach (var e in entities) Add(e);
        }

        public void Instantiate(Entity entity, int count = 1)
        {
            var parent = entity.transform.parent;
            for (int i = 0; i < count; i++)
                Add(Instantiate(entity, parent));
        }

        public void Destroy(Entity entity)
        {
            Remove(entity);
            Destroy(entity.gameObject);
        }

        public void Add(Entity entity)
        {
            if (m_Entitites.Contains(entity)) return;

            var components = entity.GetComponents<EntityComponent>();
            entity.Components.Clear();
            foreach (var c in components)
            {
                if (!entity.Components.ContainsKey(c.GetType()))
                    entity.Components.Add(c.GetType(), new HashSet<EntityComponent>());

                entity.Components[c.GetType()].Add(c);
            }

            m_Entitites.Add(entity);
#if UNITY_EDITOR
            entity.Debug();
            I(gameObject.scene).m_EntityList = m_Entitites.ToList();
#endif
        }

        public void Remove(Entity entity)
        {
            if (!m_Entitites.Contains(entity)) return;

            m_Entitites.Remove(entity);
#if UNITY_EDITOR
            I(gameObject.scene).m_EntityList = m_Entitites.ToList();
#endif
        }

        public List<T> ComponentsOfType<T>(ref List<T> listOut, bool includeInactive = true) where T : EntityComponent
        {
            listOut.Clear();
            var itemsOfT = MemPool.Get<List<T>>();
            foreach (var e in m_Entitites)
            {
                e.ComponentsOfType(ref itemsOfT, includeInactive);
                listOut.AddRange(itemsOfT);
            }

            itemsOfT.Clear();
            itemsOfT.FreeTo_MemPool();

            return listOut;
        }

        public List<Entity> With<T>(ref List<Entity> listOut, bool includeInactive = true) where T : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public List<Entity> WithAll<T0, T1>(ref List<Entity> listOut, bool includeInactive = true) where T0 : EntityComponent where T1 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public List<Entity> WithAll<T0, T1, T2>(ref List<Entity> listOut, bool includeInactive) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1, T2>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public List<Entity> WithAll<T0, T1, T2, T3>(ref List<Entity> listOut, bool includeInactive) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent where T3 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1, T2, T3>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }
    }
}