using UnityEngine;

namespace battle
{
    interface IOnStaticFound
    {
        bool OnStaticFound();
    }

    public class StaticComponentGetter<T> : MonoBehaviour where T : UnityEngine.Object
    {
        protected static T s_instance;

        public static T Instance()
        {
            if (s_instance == null)
            {
                s_instance = FindAnyObjectByType<T>();
                if (s_instance == null)
                    Debug.LogError("StaticComponentGetter FindObject returned NULL: " + typeof(T).ToString());
                else if (s_instance is IOnStaticFound)
                {
                    IOnStaticFound onStaticFound = (IOnStaticFound)s_instance;
                    if (!onStaticFound.OnStaticFound())
                    {
                        Debug.LogError("OnStaticFound failed: " + typeof(T).ToString());
                    }
                }
            }

            return s_instance;
        }
    }
}
