using UnityEditor;
using UnityEngine;

namespace Shadout.Models
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