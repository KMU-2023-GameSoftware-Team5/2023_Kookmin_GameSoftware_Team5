using battle;
using data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace deck
{
    public class SynergyUI : MonoBehaviour
    {
        public TextMeshProUGUI synergyCountText;
        public TextMeshProUGUI synergyStatText;

        public void Initialize(TraitsCount synergyClass)
        {
            synergyCountText.text = synergyCountToStirng(synergyClass);
            synergyStatText.text = synergyStatToString(synergyClass.GetTraitsStats());
        }

        public string synergyCountToStirng(TraitsCount synergyClass)
        {
            string ret = "";
            if (synergyClass.GoblinCount > 1) {
                ret += $"고블린 {synergyClass.GoblinCount}\n";
            }
            if (synergyClass.SkeletonCount > 1) {
                ret += $"스켈레톤 {synergyClass.SkeletonCount}\n";
            }
            if (synergyClass.DemonCount > 1) {
                ret += $"데몬 {synergyClass.DemonCount}\n";
            }
            if (synergyClass.HumanCount > 1){
                ret += $"휴먼 {synergyClass.HumanCount}\n";
            }
            if (synergyClass.ElfCount > 1){
                ret += $"엘프 {synergyClass.ElfCount}\n";
            }
            return ret;
        }
        public string synergyStatToString(CommonStats synergyStat)
        {
            string ret = "";
            if (synergyStat.hp > 0)
                ret += $"HP {(synergyStat.hp)}\n";
            if (synergyStat.mp > 0)
                ret += $"MP {(synergyStat.mp)}\n";
            if (synergyStat.damage > 0)
                ret += $"Damage {(synergyStat.damage)}\n";
            if (synergyStat.sheild > 0)
                ret += $"sheild  {(synergyStat.sheild)}\n";
            if (synergyStat.walkSpeed > 0)
                ret += $"walkSpeed {(synergyStat.walkSpeed)}\n";
            if (synergyStat.attackDelay > 0)
                ret += $"attackDelay {(synergyStat.walkSpeed)}\n";
            if (synergyStat.energy > 0)
                ret += $"Energy {(synergyStat.energy)}\n";
            if (synergyStat.criticalRate > 0)
                ret += $"CriticalRate {(synergyStat.criticalRate)}\n";
            return ret;
        }
    }

}
