using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// UI에 pixel character정보를 반영하는 컴포넌트
    /// </summary>
    public class CharacterIcon : MonoBehaviour
    {
        /// <summary>
        /// 캐릭터의 이름을 보여주는 UI
        /// </summary>
        public TextMeshProUGUI characterNameText;
        /// <summary>
        /// 캐릭터의 닉네임을 보여주는 UI
        /// </summary>
        public TextMeshProUGUI characterNickNameText;
        /// <summary>
        /// 캐릭터 스프라이트를 만들어주는 객체
        /// </summary>
        public SpriteBuilderForUI characterSprite;
        public CharacterDetailOpener characterDetailOpener;

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
        }

        public void setSortingOrder(int sortingOrder)
        {
            characterSprite.setSortingOrder(sortingOrder);
        }

    }
}
