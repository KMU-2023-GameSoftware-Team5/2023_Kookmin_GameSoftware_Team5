using placement;
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
            SelectCharacter characterListItem = eventData.pointerDrag.GetComponent<SelectCharacter>();

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
            Vector3[] vector3 = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(vector3);
            Debug.Log($"{vector3[0].x}/{vector3[0].y}");
            Debug.Log($"{vector3[1].x}/{vector3[1].y}");
            Debug.Log($"{vector3[2].x}/{vector3[2].y}");
            Debug.Log($"{vector3[3].x}/{vector3[3].y}");
            PlacementCharacter.minX = vector3[0].x;
            PlacementCharacter.minY = vector3[0].y;
            PlacementCharacter.maxY = vector3[1].y-1f;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
