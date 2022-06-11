using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shadout.Utilities
{
    public static class Helpers
    {
        private static PointerEventData eventDataCurrentPosition;
        private static List<RaycastResult> results;
        public static bool IsOverUi()
        {
            eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static void DeleteChildren(this Transform t)
        {
            foreach(Transform child in t) Object.Destroy(child.gameObject);
        }
    }
}