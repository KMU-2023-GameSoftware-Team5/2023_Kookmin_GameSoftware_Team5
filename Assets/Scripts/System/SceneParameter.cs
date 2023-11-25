using deck;
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

    // 승리 시 EnemyTotalLevel의 값 만큼 올라간다. 
    public int Score = 0;

    // BattleTest에서 랜덤하게 생성할 적의 래벨 총합
    private int m_enemyTotalLevel = 5;
    public int EnemyTotalLevel
    {
        get { return m_enemyTotalLevel; }
        set
        {
            if (5 <= value && value <= 45)
            {
                m_enemyTotalLevel = value;
            }
        }
    }

    private bool m_isBoss = false;
    public bool IsBoss
    {
        get { return m_isBoss; }
        set
        {
            m_isBoss = value;
        }
    }

    // battle scene result
    public bool isWin;

    public void settingEnemyLevel()
    {
        EnemyTotalLevel = 5 + PlayerManager.Instance().StageCount;
        EnemyTotalLevel += 1 * ((PlayerManager.Instance().playerBattleCount - PlayerManager.Instance().playerLoseCount) / 1); // 플레이어의 승리 1번당 적의 레벨 1 증가 
        EnemyTotalLevel += 1 * ((PlayerManager.Instance().playerWinCount / 3)); // 플레이어의 3연승당 적의 레벨 1 증가 

    }
}
