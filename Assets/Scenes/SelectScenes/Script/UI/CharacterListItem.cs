using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace jslee{
    /// <summary>
    /// 플레이어가 보유한 캐릭터 하나에 대한 정보를 보여주는 UI를 제어하는 객체
    /// </summary>
    public class CharacterListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// UI가 보여줄 캐릭터 객체
        /// </summary>
        TmpCharacter character;

        /// <summary>
        /// 드래그 처리를 위한 캔버스
        /// </summary>
        Transform canvas;
        /// <summary>
        /// 플레이어가 가진 캐릭터를 일괄하여 보여주는 UI의 위치(초기위치)
        /// </summary>
        Transform characterList;
        RectTransform rect;
        CanvasGroup canvasGroup;
        /// <summary>
        /// 캐릭터가 선택된 경우 그에 따른 캐릭터 선택 슬롯
        /// </summary>
        CharacterSelector characterSelector;

        /// <summary>
        /// 캐릭터 이미지 출력 UI
        /// </summary>
        [SerializeField]
        CharacterBuilderControl characterImage;
       
        public TmpCharacter getCharacter()
        {
            return character;
        }

        void Awake()
        {
            // canvas = FindObjectOfType<Canvas>().transform;
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// 초기화. 
        /// 1. 캐릭터에 대한 정보받아서 처리
        /// 2. 드래그 처리를 위한 UI의 Transform 설정
        /// </summary>
        /// <param name="character"></param>
        /// <param name="canvas"></param>
        /// <param name="characterList"></param>
        public void Initialize(TmpCharacter character, Transform canvas, Transform characterList)
        {
            // TODO 
            this.character = character;
            characterImage.buildCharacter(this.character.characterName);
            // characterImg.color = this.character.playerColor; 
            rect = GetComponent<RectTransform>(); 
            canvasGroup = GetComponent<CanvasGroup>(); 
            this.characterList = characterList;
            this.canvas = canvas;
        }

        /// <summary>
        /// 캐릭터 세부 정보창 열기
        /// </summary>
        /// <remarks>
        /// onClick으로 호출
        /// </remarks>
        public void openCharacterDetails()
        {
            CharacterSelectManager.Instance.openCharacterDetails(character);
        }        

        /// <summary>
        /// 캐릭터 선택 처리 메서드
        /// </summary>
        /// <remarks>
        /// 캐릭터 선택 로직에 의해 호출
        /// </remarks>
        /// <param name="characterSelector">캐릭터가 선택된 위치의 캐릭터 선택 슬롯</param>
        public void selectCharacter(CharacterSelector characterSelector)
        {
            this.characterSelector = characterSelector;
        }

        /// <summary>
        /// 캐릭터 선택 해제
        /// </summary>
        /// <remarks>
        /// 캐릭터 선택 해제 로직에서 호출
        /// </remarks>
        /// <param name="needReturn">캐릭터 인벤토리로 위치를 복귀해야하는 지에 대한 정보</param>
        public void unSelectChracter(bool needReturn)
        {
            characterSelector = null;
            if (needReturn)
            {
                returnList();
            }
        }


        /// <summary>
        /// 캐릭터가 선택된 상태인지 출력
        /// </summary>
        /// <returns>캐릭터가 선택된 상태인지 boolean값</returns>
        public bool hasSelector()
        {
            return characterSelector != null;
        }

        /// <summary>
        /// 캐릭터 swap 처리를 위한 메서드
        /// </summary>
        /// <remarks>
        /// CharacterSelector객체에서 사용됨
        /// </remarks>
        /// <param name="characterListItem"></param>
        public void swapCharacter(CharacterListItem characterListItem)
        {
            characterSelector.swapCharacter(characterListItem);
        }
        
        /// <summary>
        /// 캐릭터 선택창에서 캐릭터를 치우는 경우 호출하는 메서드
        /// </summary>
        public void removeCharacter()
        {
            characterSelector.unSelectCharacter();
            unSelectChracter(true);
        }

        /// <summary>
        /// 캐릭터의 위치를 캐릭터 선택 슬롯으로 옮기는 메서드
        /// </summary>
        /// <param name="transform"></param>
        public void setTransform(Transform transform)
        {
            rect.SetParent(transform);
            rect.position = transform.position;
        }

        /// <summary>
        /// 캐릭터 UI의 초기위치인 캐릭터 인벤토리로 복귀하는 메서드
        /// </summary>
        void returnList()
        {
            transform.SetParent(characterList);
            rect.position = characterList.position;
            //characterList.GetComponent<RectTransform>().position;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(canvas);
            transform.SetAsLastSibling();

            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.anchoredPosition = Input.mousePosition;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            
            if(transform.parent == canvas) // 최종위치가 캔버스면 복귀
            {
                returnList();
                if (characterSelector != null) // 선택창에서 드래그해서 캐릭터를 해제하는 경우
                {
                    characterSelector.unSelectCharacter();
                    unSelectChracter(false);
                }
            }
            
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true; 
        }
    }

}
