using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
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
        /// 캐릭터 인벤토리를 보여줌
        /// </summary>
        public void openCharacterInventory()
        {
            characterInventory.SetActive(true);
            itemInventory.SetActive(false);
        }

        /// <summary>
        /// 아이템 인벤토리를 보여줌
        /// </summary>
        public void openItemInventory()
        {
            characterInventory.SetActive(false);
            itemInventory.SetActive(true);
        }

    }

}
