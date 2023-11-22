using deck;
using battle;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투의 시작 및 종료를 주관하는 class
/// </summary>
public class BattleStartManager : MonoBehaviour
{
    public GameObject[] destroyTarget;
    public List<battle.PixelCharacter> characters;

    public void onClickBattleStart()
    {
        CharacterSelectManager selectSceneManager = CharacterSelectManager.Instance();

        // 아군
        characters = selectSceneManager.battleStart();
        if(characters.Count == 0)
        {
            MyDeckFactory.Instance().displayInfoMessage("최소 하나의 캐릭터를 배치해야합니다.");
            return;
        }
        // 씬 넘어가기 전 저장
        selectSceneManager.save();

        foreach (GameObject target in destroyTarget)
        {
            if(target != null)
            {
                Destroy(target);
            }
        }

        // battle scene과 연동
        BattleTest bt = BattleTest.Instance();
        bt.SetTeam0(characters.ToArray());
        BattleTest.Instance().StartBattle();
    }
}
