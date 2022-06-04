using UnityEditor;
using UnityEngine;

namespace NoName.Models
{
    public static class Developer
    {
        [MenuItem("Developer/Clear Saves")]
        public static void ClearSaves()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}