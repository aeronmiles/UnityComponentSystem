using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AM.Unity.Component.System
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class EntityManager : MonoSingletonScene<EntityManager>
    {
        [SerializeField] List<Entity> m_Entitites = new();

        public List<Entity> Entities(ref List<Entity> entities)
        {
            entities.Clear();
            entities.AddRange(m_Entitites);
            return entities;
        }

        private void OnEnable()
        {
            getSceneEntities();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!Application.isPlaying)
                getSceneEntities();
        }
#endif

        void getSceneEntities()
        {
            m_Entitites.Clear();
            var entities = FindObjectsOfType<Entity>(true);
            foreach (var e in entities) Add(e);
        }

        public void Instantiate(Entity entity, int count = 1)
        {
            var parent = entity.transform.parent;
            for (int i = 0; i < count; i++)
                Add(Instantiate(entity, parent));
        }

        internal void Destroy(Entity entity)
        {
            Remove(entity);
            Destroy(entity.gameObject);
        }

        internal void Add(Entity entity)
        {
            entity.UpdateComponents();
            if (m_Entitites.Contains(entity)) return;

            m_Entitites.Add(entity);
        }

        internal void Remove(Entity entity)
        {
            if (!m_Entitites.Contains(entity)) return;

            m_Entitites.Remove(entity);
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
                if (e.HasComponent<T>())
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