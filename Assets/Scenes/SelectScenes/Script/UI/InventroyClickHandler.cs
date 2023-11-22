using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 플레이어가 보유한 캐릭터, 아이템 보여주는 창에서 탭버튼 조작 이벤트 관리 컴포넌트
    /// </summary>
    public class InventroyClickHandler : MonoBehaviour
    {
        /// <summary>
        /// 캐릭터 인벤토리 UI
        /// </summary>
        [SerializeField]
        GameObject characterInventory;
        /// <summary>
        /// 아이템 인벤토리 UI
        /// </summary>
        [SerializeField]
        GameObject itemInventory;
        /// <summary>
        /// 선택된 캐릭터 보이기
        /// </summary>
        [SerializeField]
        GameObject selectedCharacter;

        void unActive()
        {
            characterInventory.SetActive(false);
            itemInventory.SetActive(false);
            selectedCharacter.SetActive(false);
        }

        /// <summary>
        /// 캐릭터 인벤토리를 보여줌
        /// </summary>
        public void openCharacterInventory()
        {
            unActive();
            characterInventory.SetActive(true);
        }

        /// <summary>
        /// 아이템 인벤토리를 보여줌
        /// </summary>
        public void openItemInventory()
        {
            unActive();
            itemInventory.SetActive(true);
        }
        /// <summary>
        /// 선택된 캐릭터를 보여줌
        /// </summary>
        public void openSelectedCharacter()
        {
            unActive();
            selectedCharacter.SetActive(true);
        }
    }

}
