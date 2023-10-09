using deck;
using lee;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartManager : MonoBehaviour
{
    public GameObject[] destroyTarget;
    public lee.PixelCharacter[] characters;

    public void onClickBattleStart()
    {
        // 아군
        characters = CharacterSelectManager.Instance().battleStart();
        
        foreach (GameObject target in destroyTarget)
        {
            if(target != null)
            {
                Destroy(target);
            }
        }

        // battle scene과 연동
        BattleTest bt = BattleTest.Instance();
        bt.SetTeam0(characters);
        BattleTest.Instance().StartBattle();
    }
}
