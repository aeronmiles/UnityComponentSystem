using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityComponent : MonoBehaviour
    {
        private void Awake()
        {
            EntityManager.Add(GetComponent<Entity>());
        }

        private void OnDestroy()
        {
            EntityManager.Remove(GetComponent<Entity>());
        }
    }

    public interface IFixedUpdate
    {
        void IFixedUpdate();
    }

    public interface IUpdate
    {
        void IUpdate();
    }

    public interface ILateUpdate
    {
        void ILateUpdate();
    }

    public static class InterfaceExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IFixedUpdate<T>(this IEnumerable<T> items) where T : IFixedUpdate
        {
            foreach (var i in items) i.IFixedUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IUpdate<T>(this IEnumerable<T> items) where T : IUpdate
        {
            foreach (var i in items) i.IUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ILateUpdate<T>(this IEnumerable<T> items) where T : ILateUpdate
        {
            foreach (var i in items) i.ILateUpdate();
        }
    }
}