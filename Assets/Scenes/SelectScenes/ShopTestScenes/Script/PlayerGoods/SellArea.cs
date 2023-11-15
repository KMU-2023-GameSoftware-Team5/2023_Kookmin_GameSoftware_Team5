using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    public class SellArea : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// 플레이어의 캐릭터, 아이템이 여기에 드롭되었을 때 판매를 진행한다.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            PlayerCharacterGoods characterGoods = eventData.pointerDrag.GetComponent<PlayerCharacterGoods>();

            if (characterGoods != null) // 캐릭터 정보 UI가 아닌 경우 
            {
                characterGoods.sellCharacter();
            }
        }
    }
}
