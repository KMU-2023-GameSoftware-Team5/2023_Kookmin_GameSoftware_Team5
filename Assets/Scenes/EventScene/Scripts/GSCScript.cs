using UnityEngine;

namespace GSC
{
    [CreateAssetMenu(menuName = "GSCript", order = 2)]
    public class GSCScript : ScriptableObject
    {
        public Sprite image;

        [TextArea(10, 200)]
        public string script;
    }
}
