using battle;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 플레이어가 보유한 캐릭터 하나에 대한 정보를 보여주는 UI를 제어하는 객체
    /// </summary>
    public class CharacterListItem: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// UI가 보여줄 캐릭터 객체
        /// </summary>
        PixelCharacter character;

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
        /// 캐릭터 이미지 출력 UI
        /// </summary>
        SpriteBuilderForUI characterImage;

        [SerializeField]GameObject placeMark;

        /// <summary>
        /// select Scene에서 이 캐릭터가 이미 배치되어 있는지 
        /// </summary>
        bool placeFlag = false;
        public bool isPlaced
        {
            get {  return placeFlag; }
            set
            {
                placeFlag = value;
                placeMark.SetActive(value);
            }
        }

        public PixelCharacter getCharacter()
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
        /// <param name="character">이 UI가 보여줄 캐릭터객체</param>
        /// <param name="canvas">drag하는 동안 위치해있을 canvas</param>
        /// <param name="characterList">캐릭터 객체가 원래 위치할 리스트</param>
        public void Initialize(PixelCharacter character, Transform canvas, Transform characterList, SpriteBuilderForUI characterImage)
        {
            // characterImg.color = this.character.playerColor; 
            rect = GetComponent<RectTransform>(); 
            canvasGroup = GetComponent<CanvasGroup>(); 
            this.character = character;
            this.characterList = characterList;
            this.canvas = canvas;
            this.characterImage = characterImage;
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

            // for sprite maks
            characterImage.setSortingOrder(1);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(canvas);
            transform.SetAsLastSibling();

            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = false;

            // for sprite Mask
            characterImage.setSortingOrder(72);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.anchoredPosition = Input.mousePosition;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            returnList();
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        }

        public bool compareCharacter(PixelCharacter character)
        {
            return this.character.ID == character.ID;
        }

    }

}
