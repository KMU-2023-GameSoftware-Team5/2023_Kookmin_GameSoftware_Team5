using deck;
using System;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;

namespace placement
{
    /// <summary>
    /// 캐릭터 배치 컴포넌트 
    /// </summary>
    /// <remarks>Drag와 배치 위치 처리</remarks>
    public class PlacementCharacter : MonoBehaviour
    {

        [SerializeField] Transform characterPosition;
        [SerializeField] GameObject pixelHumanoid;
        /// <summary>
        /// 배치된 좌표값을 캐릭터에게 배치
        /// </summary>
        PixelCharacter pixelCharacter;

        /// <summary>
        /// 배치시 캐릭터 위에 위치할 이름
        /// </summary>
        public PlacementCharacterHeadName headName;


        // Drag가능한 영역
        public static float maxX = 0.0f;
        public static float minX = -6.0f;
        public static float maxY = 3.0f;
        public static float minY = -4.0f;
        
        public bool dragMode { private get;  set; }

        void OnMouseDrag()
        {
            if (!dragMode) {
                return;
            }
            Vector3 mousePosition = new Vector3 (
                Input.mousePosition.x,
                Input.mousePosition.y,
                0);
            characterPosition.position = Camera.main.ScreenToWorldPoint(mousePosition);
            characterPosition.position = setDragAbleArea(characterPosition.position);
        }

        void OnMouseUp()
        {
            pixelCharacter.worldPosition = characterPosition.position;
        }


        /// <summary>
        /// 배치가능한 영역으로 오브젝트의 위치를 제한함
        /// </summary>
        /// <param name="position">현재 위치</param>
        /// <returns>배치가능한 영역으로 제한한 위치</returns>
        Vector3 setDragAbleArea(Vector3 position)
        {
            Vector3 ret = new Vector3(
                position.x,
                position.y,
                position.z
            );
            ret.z = 0;
            if(ret.x > maxX )
            {
                ret.x = maxX;
            }
            else if (ret.x < minX)
            {
                ret.x = minX;
            }
            if (ret.y > maxY ) { 
                ret.y = maxY;
            }
            else if(ret.y < minY)
            {
                ret.y = minY;
            }
            return ret;
        }

        /// <summary>
        /// 캐릭터 배치 컴포넌트 초기화 
        /// </summary>
        /// <param name="pixelCharacter">캐릭터 배치 위치 저장을 위한 레퍼런스</param>
        public void Initialize(PixelCharacter pixelCharacter)
        {
            this.pixelCharacter = pixelCharacter;
            characterPosition.position = setDragAbleArea(characterPosition.position);
        }

        /// <summary>
        /// 캐릭터 언셀렉트시 배치된 캐릭터 오브젝트를 삭제하는 메서드
        /// </summary>
        public void unSelect()
        {
            headName.unSelect();
            Destroy(pixelHumanoid);
        }

        /// <summary>
        /// SelectScene에서만 사용하는 HeadName, PlacementCharacter 삭제
        /// </summary>
        public void battleStart()
        {
            headName.battleStart();
            Destroy(this);
        }

        public bool compareCharacter(PixelCharacter character)
        {
            return pixelCharacter.ID == character.ID;
        }
    }
}
