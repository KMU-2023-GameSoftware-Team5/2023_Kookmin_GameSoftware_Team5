using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace deck
{
    public class LightCharacterListItem : MonoBehaviour
    {
        CharacterListItem characterListItem;
        /// <summary>
        /// 캐릭터 이미지 출력 UI
        /// </summary>
        [SerializeField]
        SpriteBuilderForUI characterImage;
        /// <summary>
        /// UI가 보여줄 캐릭터 객체
        /// </summary>
        public PixelCharacter character { get; private set; }
        /// <summary>
        /// 캐릭터 이름
        /// </summary>
        [SerializeField] TextMeshProUGUI characterName;

        public void Initialize(PixelCharacter character)
        {
            this.character = character;
            characterImage.buildCharacter(this.character.characterName);
            characterName.text = character.characterNickName;
            characterListItem = GetComponent<CharacterListItem>();
            Destroy(characterListItem);
        }

        public void Initialize(PixelCharacter character, Transform canvas, Transform characterList)
        {
            this.character = character;
            characterImage.buildCharacter(this.character.characterName);
            characterName.text = character.characterNickName;
            characterListItem = GetComponent<CharacterListItem>();
            characterListItem.Initialize(character, canvas, characterList, characterImage);
        }

        public void setSortingOrder(int sortingOrder)
        {
            characterImage.setSortingOrder(sortingOrder);
        }

    }
}
