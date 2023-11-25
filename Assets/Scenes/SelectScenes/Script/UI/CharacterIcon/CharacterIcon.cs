using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// UI에 pixel character정보를 반영하는 컴포넌트
    /// </summary>
    public class CharacterIcon : MonoBehaviour
    {
        [Header("Character name-nickname")]
        /// <summary>
        /// 캐릭터의 이름을 보여주는 UI
        /// </summary>
        public TextMeshProUGUI characterNameText;
        /// <summary>
        /// 캐릭터의 닉네임을 보여주는 UI
        /// </summary>
        public TextMeshProUGUI characterNickNameText;
        
        /// <summary>
        /// 미사용, 추후 삭제 예정
        /// </summary>
        SpriteBuilderForUI characterSprite;

        [Header("CharacterSprite")]
        public CharacterSpriteLoader characterSpriteLoader;
        [Header("Detail Opener Component")]
        public CharacterDetailOpener characterDetailOpener;

        
        [Header("character upgrade")]
        public TextMeshProUGUI characterTierText;
        public Image characterTierColor;
        public float alpha = 0.78f;

        /// <summary>
        /// 이 객체가 보여줄 캐릭터 객체
        /// </summary>
        public PixelCharacter character { get; private set; }

        /// <summary>
        /// 초기화함수. 캐릭터를 인자로 받아서 UI컴포넌트에 캐릭터 정보를 집어넣음
        /// </summary>
        /// <param name="character">보여줄 캐릭터</param>
        public void Initialize(PixelCharacter character)
        {
            this.character = character;
            if (characterSprite != null)
            {
                characterSprite.buildCharacter(character.characterName);
            }
            if (characterNickNameText != null)
            {
                characterNickNameText.text = character.characterNickName;
            }
            if (characterNameText != null)
            {
                characterNameText.text = character.characterName;
            }
            if (characterDetailOpener != null)
            {
                characterDetailOpener.character = character;
            }
            if (characterSpriteLoader != null)
            {
                characterSpriteLoader.loadCharacterSprite(character.characterName);
            }
            if (characterTierText != null)
            {
                characterTierText.text = $"★{character.tier}";
            }
            if (characterTierColor != null && character.tier != 1)
            {
                Color color = MyDeckFactory.Instance().tierColors[character.tier - 1];
                color.a = alpha;
                characterTierColor.color = color;
            }
            MyDeckFactory.Instance().nickNameChangeEvent.AddListener(onNickNameChange);
        }

        public void onNickNameChange(string id)
        {
            if(character.ID == id)
            {
                if (characterNickNameText != null)
                {
                    characterNickNameText.text = character.characterNickName;
                }            
            }
        }

        /// <summary>
        /// 미사용
        /// </summary>
        /// <param name="sortingOrder"></param>
        public void setSortingOrder(int sortingOrder)
        {
            if(characterSprite != null)
                characterSprite.setSortingOrder(sortingOrder);
        }

    }
}
