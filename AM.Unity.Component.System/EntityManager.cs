using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AM.Unity.Component.System
{
    public class EntityManager : MonoSingletonScene<EntityManager>
    {
        [Header("Debug")]
        [SerializeField] List<Entity> m_EntityList;
        static HashSet<Entity> m_Entitites = new();

        private void OnEnable()
        {
            m_Entitites.Clear();
            var entities = GetComponentsInChildren<Entity>(true);
            foreach (var e in entities) Add(e);
        }

        static List<IFixedUpdate> m_FixedUpdateEntities = new();
        private void FixedUpdate()
        {
            m_FixedUpdateEntities.IFixedUpdate();
        }

        static List<IUpdate> m_UpdateEntities = new();
        private void Update()
        {
            m_UpdateEntities.IUpdate();
        }

        static List<ILateUpdate> m_LateUpdateEntities = new();
        private void LateUpdate()
        {
            m_LateUpdateEntities.ILateUpdate();
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
            foreach (var c in components)
            {
                if (!entity.Components.ContainsKey(c.GetType()))
                    entity.Components.Add(c.GetType(), new HashSet<EntityComponent>());

                entity.Components[c.GetType()].Add(c);
            }

            foreach (var c in entity.Components)
            {
                foreach (var i in c.Value)
                {
                    if ((i as IFixedUpdate) != null)
                        m_FixedUpdateEntities.Add(i as IFixedUpdate);
                    if ((i as IUpdate) != null)
                        m_UpdateEntities.Add(i as IUpdate);
                    if ((i as ILateUpdate) != null)
                        m_LateUpdateEntities.Add(i as ILateUpdate);
                }
            }

            m_Entitites.Add(entity);
#if UNITY_EDITOR
            entity.Debug();
            I.m_EntityList = m_Entitites.ToList();
#endif
        }

        public static void Remove(Entity entity)
        {
            if (!m_Entitites.Contains(entity)) return;

            foreach (var c in entity.Components)
            {
                foreach (var i in c.Value)
                {
                    m_FixedUpdateEntities.Remove(i as IFixedUpdate);
                    m_UpdateEntities.Remove(i as IUpdate);
                    m_LateUpdateEntities.Remove(i as ILateUpdate);
                }
            }

            m_Entitites.Remove(entity);
#if UNITY_EDITOR
            I.m_EntityList = m_Entitites.ToList();
#endif
        }

        public static List<T> ComponentsOfType<T>(ref List<T> listOut, bool includeInactive = true) where T : EntityComponent
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

        public static List<Entity> With<T>(ref List<Entity> listOut, bool includeInactive = true) where T : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public static List<Entity> WithAll<T0, T1>(ref List<Entity> listOut, bool includeInactive = true) where T0 : EntityComponent where T1 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public static List<Entity> WithAll<T0, T1, T2>(ref List<Entity> listOut, bool includeInactive) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1, T2>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }

        public static List<Entity> WithAll<T0, T1, T2, T3>(ref List<Entity> listOut, bool includeInactive) where T0 : EntityComponent where T1 : EntityComponent where T2 : EntityComponent where T3 : EntityComponent
        {
            listOut.Clear();
            foreach (var e in m_Entitites)
                if (e.HasComponents<T0, T1, T2, T3>())
                    if (includeInactive || e.gameObject.activeInHierarchy) listOut.Add(e);

            return listOut;
        }
    }
}