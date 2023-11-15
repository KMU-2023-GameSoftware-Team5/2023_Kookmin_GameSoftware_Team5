using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace deck
{
    public class PlayerCharacterGoods : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("CharacterInfoUI")]
        [SerializeField] CharacterIcon characterInfo;

        /// <summary>
        /// 캐릭터 가격
        /// </summary>
        [SerializeField] TextMeshProUGUI priceTextUI;

        /// <summary>
        /// 판매할 캐릭터
        /// </summary>
        public PixelCharacter character { get; private set; }

        /// <summary>
        /// 판매할 캐릭터 가격 
        /// </summary>
        int price;

        public Transform slot;

        Transform dragCanvas;
        RectTransform rect;
        CanvasGroup canvasGroup;

        public Outline[] outlines;

        public void Initialize(PixelCharacter character, int price, Transform dragCanvas)
        {
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            this.character = character;
            characterInfo.Initialize(character);
            this.price = character.tier;
            priceTextUI.text = $"${character.tier}";
            this.dragCanvas = dragCanvas;
            foreach (var outline in outlines)
            {
                Color _color = MyDeckFactory.Instance().tierColors[character.tier - 1];
                _color.a = 0.5f;
                outline.effectColor = _color;
            }
        }

        public void destroy()
        {
            Destroy(slot.gameObject);
        }
        void returnList()
        {
            transform.SetParent(slot);
            rect.position = slot.position;
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

        public void sellCharacter()
        {
            ShopSceneManager.Instance().sellCharacter(character, price);
        }

    }

}
