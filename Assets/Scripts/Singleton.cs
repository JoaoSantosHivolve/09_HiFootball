//============================ HIVOLVE ========================================
// Author: João Santos;
// Purpose: Used on classes that are only instantiated once by scene.
//=============================================================================

using UnityEngine;

namespace Common
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(Instance);
            }
            else
            {
                Debug.Log("Multiple Objects " + typeof(T).Name);
                Destroy(this.gameObject);
            }
        }
    }

    public class SingletonDestroyable<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.Log("Multiple Objects " + typeof(T).Name);
                Destroy(this.gameObject);
            }
        }
    }
}