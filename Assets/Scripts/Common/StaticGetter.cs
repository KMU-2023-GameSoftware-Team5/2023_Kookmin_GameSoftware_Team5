using battle;
using UnityEngine;

public class StaticGetter<T> where T : new()
{
    protected static T s_instance;

    public static T Instance()
    {
        if (s_instance == null)
        {
            s_instance = new T();
            if (s_instance == null)
                Debug.LogError("StaticGetter new returned NULL: " + typeof(T).ToString());
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
