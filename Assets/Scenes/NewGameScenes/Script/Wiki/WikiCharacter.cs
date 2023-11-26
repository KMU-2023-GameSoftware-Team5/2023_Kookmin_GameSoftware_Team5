using battle;
using data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class WikiCharacter : MonoBehaviour
    {
        public CharacterSpriteLoader wikiImage;
        public TextMeshProUGUI characterName;
        [Header("attackType")]
        public TextMeshProUGUI attackTypeText;
        public Image attackTypeImage;

        [Header("skill")]
        public TextMeshProUGUI skillText;
        public Image skillImage;

        int idx;
        WikiManager wikiManager;

        public void Initialize(int idx, PixelHumanoidData character, WikiManager wikiManager)
        {
            this.wikiImage.loadCharacterSprite(character.characterName);
            this.characterName.text = character.characterName;
            this.idx = idx;

            // setting attackType
            attackTypeText.text = character.defualtAttackType.ToString();
            attackTypeImage.sprite = MyDeckFactory.Instance().attackTypeDescriptionMap[character.defualtAttackType].img;

            // setting skill
            skillText.text = character.customSkillName;
            skillImage.sprite = StaticLoader.Instance().GetCustomSkillData(character.customSkillName).skillIcon;
        }

        public void onClickWikiDetail()
        {
            Debug.Log("openWikiDetail");
        }
    }

}
