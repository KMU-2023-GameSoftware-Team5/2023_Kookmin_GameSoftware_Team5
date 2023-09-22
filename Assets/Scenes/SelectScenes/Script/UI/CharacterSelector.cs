using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jslee
{
    /// <summary>
    /// 캐릭터 선택 슬롯 컴포넌트 객체
    /// </summary>
    public class CharacterSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        /// <summary>
        /// 캐릭터 선택 처리 관련 슬롯 고유 id 
        /// </summary>
        int selectId;
        Image image;    
        RectTransform rectTransform;
        CharacterListItem characterListItem;

        void Start()
        {

        }

        private void Awake()
        {
            image = GetComponent<Image>();
            image.color = Color.black;
            rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(int selectId)
        {
            this.selectId = selectId;
        }


        public void setCharacter(CharacterListItem characterListItem)
        {        
            this.characterListItem = characterListItem;
        }

        /// <summary>
        /// 사용자 조작에 의해 캐릭터의 선택 슬롯 위치가 교환되었을 경우 처리하는 메서드
        /// </summary>
        /// <remarks>
        /// 1번슬롯의 A캐릭터가 2번슬롯(B캐릭터 선택중)으로 이동하였을 경우 1번 슬롯의 swapCharacter메서드에 B캐릭터를 매개변수로 전달하여 호출할 것 
        /// </remarks>
        /// <param name="newCharacterListItem"></param>
        public void swapCharacter(CharacterListItem newCharacterListItem)
        {
            // 기존 캐릭터 선택 해제 
            characterListItem.unSelectChracter(false);
            unSelectCharacter();

            // 새 캐릭터 선택 설정
            selectCharacter(newCharacterListItem);
        }

        /// <summary>
        /// 캐릭터 선택 처리
        /// </summary>
        /// <param name="newCharacterListItem"></param>
        public void selectCharacter(CharacterListItem newCharacterListItem)
        {
            characterListItem = newCharacterListItem;
            characterListItem.selectCharacter(this);
            characterListItem.setTransform(rectTransform);
            CharacterSelectManager.Instance.selectCharacter(selectId, characterListItem.getCharacter());
        }

        /// <summary>
        /// 캐릭터 선택 해제 처리
        /// </summary>
        public void unSelectCharacter()
        {
            characterListItem = null;
            CharacterSelectManager.Instance.unSelectCharacter(selectId);
        }

        /// <summary>
        /// 캐릭터 정보 아이콘이 드래그해서 슬롯에 도달하면 이벤트 처리
        /// </summary>
        /// <param name="eventData"></param>
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag != null)
            {
                CharacterListItem newCharacterListItem = eventData.pointerDrag.GetComponent<CharacterListItem>(); ;

                if(newCharacterListItem == null) // 캐릭터 정보 아이콘이 아닌 경우 
                {
                    return;
                }
                if(characterListItem != null) // 기존에 선택된 캐릭터가 있는 경우 
                {
                    if (newCharacterListItem.hasSelector()) // 새로 선택될 캐릭터가 이미 선택된 상태인 경우 -> SWAP
                    {
                        newCharacterListItem.swapCharacter(characterListItem);
                        unSelectCharacter();
                        newCharacterListItem.unSelectChracter(false);
                    }
                    else // 새로 선택될 캐릭터가 선택되지 않은 상태인 경우 -> 현재 선택된 캐릭터만 제거
                    {
                        characterListItem.unSelectChracter(true);
                        unSelectCharacter();
                    }
                }
                else if (newCharacterListItem.hasSelector()) // 새로 선택될 캐릭터가 이미 선택된 상태인 경우 -> 기존 선택 설정을 해제
                {
                    newCharacterListItem.removeCharacter();
                }
                selectCharacter(newCharacterListItem);
            }
        }

        // 이하는 별로 중요하지 않은 :hover 처리 메서드
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            image.color = Color.grey;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            image.color = Color.black;
        }

    }
}
