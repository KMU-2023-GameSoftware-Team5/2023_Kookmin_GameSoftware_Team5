using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    /// <summary>
    /// SelectScene에서 캐릭터를 드래그해서 배치하는 역할
    /// </summary>
    public class SelectCharacter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// UI가 보여줄 캐릭터 객체
        /// </summary>
        public PixelCharacter character { get; private set; }

        /// <summary>
        /// 드래그 처리를 위한 캔버스
        /// </summary>
        Transform dragCanvas;
        RectTransform rect;
        CanvasGroup canvasGroup;

        /// <summary>
        /// 캐릭터 정보 출력 UI
        /// </summary>
        [SerializeField]CharacterIcon characterIcon;

        /// <summary>
        /// 초기위치 
        /// </summary>
        [SerializeField] Transform characterSlot;

        /// <summary>
        /// 배치 되었을 때 표시하는 UI
        /// </summary>
        [SerializeField] GameObject placeMark;

        /// <summary>
        /// select Scene에서 이 캐릭터가 이미 배치되어 있는지 
        /// </summary>
        bool placeFlag = false;
        public bool isPlaced
        {
            get { return placeFlag; }
            set
            {
                placeFlag = value;
                if (placeMark)
                {
                    placeMark.SetActive(value);
                }
            }
        }

        void Awake()
        {
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
        public void Initialize(PixelCharacter character, Transform canvas)
        {
            // characterImg.color = this.character.playerColor; 
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            this.character = character;
            this.dragCanvas = canvas;
            characterIcon.Initialize(character);
        }

        /// <summary>
        /// 캐릭터 UI의 초기위치인 캐릭터 인벤토리로 복귀하는 메서드
        /// </summary>
        void returnList()
        {
            transform.SetParent(characterSlot);
            rect.position = characterSlot.position;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(dragCanvas);
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
            returnList();
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        }

    }

}
