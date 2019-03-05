using UnityEngine;

namespace Unity.Entities
{
    [ExecuteAlways]
    class PlayerLoopDisableManager : MonoBehaviour
    {
        public bool IsActive;

        public void OnEnable()
        {
            if (!IsActive)
                return;

            IsActive = false;
            DestroyImmediate(gameObject);
        }

        public void OnDisable()
        {
            if (IsActive)
                PlayerLoopManager.InvokeBeforeDomainUnload();
        }
    }
}
