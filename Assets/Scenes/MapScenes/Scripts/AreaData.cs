using UnityEngine;
using UnityEngine.Events;

namespace GameMap
{
    [CreateAssetMenu(menuName = "Map Area", order = 3)]
    public class AreaData : ScriptableObject
    {
        public string areaName;
        public Sprite sprite;
        public UnityEvent onClick;

        public static AreaData GetAreaDataByName(string name)
        {
            var temp = FindObjectsOfType<AreaData>();

            foreach (AreaData t in temp)
                if (t.areaName == name)
                    return t;

            return null;
        }
    }
}
