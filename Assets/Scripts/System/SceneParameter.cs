using GameMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// acess singleton by SceneParamter.Instance()
public class SceneParamter : StaticGetter<SceneParamter>
{
    private MobSetData m_mobSet;
    public MobSetData MobSet
    {
        get { return m_mobSet; }
        set
        {
            if (value != null)
            {
                m_mobSet = value;
            }
        }
    }

    // == Map parameters ==
    public int MapStage = 1;
    public int EnemyReinforce;

    // battle scene result
    public bool isWin;
}
