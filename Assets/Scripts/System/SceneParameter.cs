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
    public bool NeedInitializeMapParameter = true;

    // This array saves the AreaData's that areas of map have in order
    public AreaData[] AreaDatas;
    public int AreaVisitCount;
    public int NowAreaIndex = -1;

    public void InitializeMapParameters()
    {
        AreaDatas = null;
        AreaVisitCount = 0;
        NowAreaIndex = -1;

        NeedInitializeMapParameter = false;
    }
}
