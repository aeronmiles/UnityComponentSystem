using UnityEngine;

namespace AM.Unity.Component.System
{
    public class Benchmarks : MonoBehaviour
    {
        [SerializeField] int m_Count = 1000;
        private void OnEnable()
        {
            var entity = FindObjectOfType<Entity>();
            EntityManager.I(gameObject.scene).Instantiate(entity, m_Count);
        }
    }
}
