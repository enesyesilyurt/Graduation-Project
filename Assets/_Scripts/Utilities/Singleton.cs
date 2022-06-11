using UnityEngine;

namespace Shadout.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static volatile T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;
                }
                return instance;
            }
        }

        private void OnApplicationQuit() 
        {
            instance = null;
            Destroy(gameObject);
        }
    }
}