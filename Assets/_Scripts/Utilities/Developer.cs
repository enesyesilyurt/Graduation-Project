using UnityEditor;
using UnityEngine;

namespace Shadout.Models
{
    public static class Developer
    {
        #if UNITY_EDITOR
        [MenuItem("Developer/Clear Saves")]
        public static void ClearSaves()
        {
            PlayerPrefs.DeleteAll();
        }
        #endif
    }
}