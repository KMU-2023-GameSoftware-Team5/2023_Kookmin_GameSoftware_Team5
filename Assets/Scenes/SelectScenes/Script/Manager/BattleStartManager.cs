using deck;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartManager : MonoBehaviour
{
    public GameObject[] destroyTarget;

    public void onClickBattleStart()
    {
        CharacterSelectManager.Instance().battleStart();
        foreach (GameObject target in destroyTarget)
        {
            if(target != null)
            {
                Destroy(target);
            }
        }
    }
}
