using deck;
using battle;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartManager : MonoBehaviour
{
    public GameObject[] destroyTarget;
    public battle.PixelCharacter[] characters;

    public void onClickBattleStart()
    {
        // 씬 넘어가기 전 저장
        CharacterSelectManager selectSceneManager = CharacterSelectManager.Instance();
        selectSceneManager.save();
        Debug.Log("select Scene 저장 완료");

        // 아군
        characters = selectSceneManager.battleStart();
        
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
