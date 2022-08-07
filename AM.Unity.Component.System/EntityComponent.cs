using System.Collections.Generic;
using UnityEngine;

namespace AM.Unity.Component.System
{
    [RequireComponent(typeof(Entity))]
    public abstract class EntityComponent : MonoBehaviour
    {
        [Header("Debug")]
        public bool IsActive = true;
        
        private void Awake()
        {
            EntityManager.Add(GetComponent<Entity>());
        }

        private void OnDestroy()
        {
            EntityManager.Remove(GetComponent<Entity>());
        }
    }
}