using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    /// <summary>
    /// 배치가능영역 - UI를 Drag해서 Drop하는 순간에 배치가능한 영역을 의미한다.
    /// </summary>
    public class PlaceableArea : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// 이 영역안에 UI가 드래그 후 드롭이 일어날 때 해당 위치에 캐릭터를 배치한다
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            CharacterListItem characterListItem = eventData.pointerDrag.GetComponent<CharacterListItem>();

            if (characterListItem == null) // 캐릭터 정보 UI가 아닌 경우 
            {
                return;
            }else if (!characterListItem.isPlaced) {
                Vector3 mousePosition = new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                0);
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                CharacterSelectManager.Instance().placeCharacter(characterListItem, mousePosition);
            }
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
