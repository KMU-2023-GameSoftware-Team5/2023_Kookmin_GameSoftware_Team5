using battle;
using data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class CharacterDetailDesc : MonoBehaviour
    {
        [Header("Attack Type")]
        public TextMeshProUGUI attackTypeTitle;
        public TextMeshProUGUI attackTypeText;
        public Image atIcon;
        [Header("Skill")]
        public TextMeshProUGUI skillTitle;
        public TextMeshProUGUI skillText;
        public Image skillIcon;
        Dictionary<EDefualtAttackType, AttackTypeDescription> attackTypeDescriptionMap;
        Dictionary<battle.PixelHumanoid.ESkill, string> skillDescriptionMap;

        private void Awake()
        {
            attackTypeDescriptionMap = MyDeckFactory.Instance().attackTypeDescriptionMap;
            skillDescriptionMap = MyDeckFactory.Instance().skillDescriptionMap;
        }

        public void openCharacterDesc(PixelCharacter pixelCharacter)
        {
            attackTypeTitle.text = pixelCharacter.defualtAttackType.ToString();
            
            if (attackTypeDescriptionMap.ContainsKey(pixelCharacter.defualtAttackType))
            {
                
                attackTypeText.text = attackTypeDescriptionMap[pixelCharacter.defualtAttackType].desc;
                atIcon.sprite = attackTypeDescriptionMap[pixelCharacter.defualtAttackType].img;
            }
            else
            {
                attackTypeText.text = "정보가 없습니다.";
            }


            skillTitle.text = pixelCharacter.skill.ToString();
            CustomSkillData skill = StaticLoader.Instance().GetCustomSkillData(pixelCharacter.skill);

            if (skill != null) 
            {
                skillText.text = skill.skillDescription;
                if(skill.skillIcon != null)
                    skillIcon.sprite = skill.skillIcon;
            }
            else
            {
                skillText.text = "정보가 없습니다.";
            }
        }
    }
}
