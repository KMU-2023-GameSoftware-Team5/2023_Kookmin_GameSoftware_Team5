using data;
using deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 캐릭터 스텟창 전체 관리
    /// </summary>
    public class CharacterDetailsStatUI : MonoBehaviour
    {
        PixelCharacter character;

        [SerializeField] CharacterDetailsStatUIitem hp;
        [SerializeField] CharacterDetailsStatUIitem mp;
        [SerializeField] CharacterDetailsStatUIitem damage;
        [SerializeField] CharacterDetailsStatUIitem sheild;
        [SerializeField] CharacterDetailsStatUIitem walkSpeed;
        [SerializeField] CharacterDetailsStatUIitem attackDelay;
        [SerializeField] CharacterDetailsStatUIitem energy;
        [SerializeField] CharacterDetailsStatUIitem criticalRate;

        /// <summary>
        /// 캐릭터 스텟 변동사항이 있는 경우 호출되어 캐릭터 스텟을 출력하는 메서드
        /// </summary>
        public void updateStat()
        {
            Debug.Log("Update Stat");
            int logs = 0;
            foreach(EquipItem item in this.character.Inventory)
            {
                if(item != null)
                {
                    logs++;
                    Debug.Log($"energy : {item.itemStat.energy.ToString()}");
                }
            }
            Debug.Log(logs);
            CommonStats itemStat = character.getEquipItemStats();
            CommonStats characterStat = character.getCharacterStats();
            setStatText(characterStat, itemStat);
        }

        /// <summary>
        /// 스텟을 보여줄 캐릭터를 설정
        /// </summary>
        /// <param name="character"></param>
        public void setCharacter(PixelCharacter character)
        {
            this.character = character;
            updateStat();
        }

        /// <summary>
        /// 스텟을 계산하여 외부에 외부에 출력하는 메서드
        /// </summary>
        /// <param name="characterStat">캐릭터의 종합 스텟</param>
        /// <param name="itemStat">아이템에 의해 증감되는 스텟량</param>
        public void setStatText(CommonStats characterStat, CommonStats itemStat)
        {
            hp.setStat(characterStat.hp, itemStat.hp);
            mp.setStat(characterStat.mp, itemStat.mp);
            damage.setStat(characterStat.damage, itemStat.damage);
            sheild.setStat(characterStat.sheild, itemStat.sheild);
            walkSpeed.setStat(characterStat.walkSpeed, itemStat.walkSpeed);
            attackDelay.setStat(characterStat.attackDelay, itemStat.attackDelay);
            energy.setStat(characterStat.energy, itemStat.energy);
            criticalRate.setStat(characterStat.criticalRate, itemStat.criticalRate);
        }
        
        void Update()
        {

        }
    }
}

