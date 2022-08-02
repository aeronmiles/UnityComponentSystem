using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AM.Unity.Component.System
{
    public class Benchmarks : MonoBehaviour
    {
        [SerializeField] int m_Count = 1000;
        private void OnEnable()
        {
            var entity = FindObjectOfType<Entity>();
            EntityManager.Instantiate(entity, m_Count);
        }
    }
}
