using deck;
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
        PixelCharacter pixelCharacter;

        void OnMouseDrag()
        {
            Vector3 mousePosition = new Vector3 (
                Input.mousePosition.x,
                Input.mousePosition.y,
                10);
            characterPosition.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        void OnMouseUp()
        {
            pixelCharacter.worldPosition = characterPosition.position;
        }

        /// <summary>
        /// 캐릭터 배치 컴포넌트 초기화 
        /// </summary>
        /// <param name="pixelCharacter">캐릭터 배치 위치 저장을 위한 레퍼런스</param>
        public void Initialize(PixelCharacter pixelCharacter)
        {
            this.pixelCharacter = pixelCharacter;
        }

        /// <summary>
        /// 캐릭터 언셀렉트시 배치된 캐릭터 오브젝트를 삭제하는 메서드
        /// </summary>
        public void unSelect()
        {
            Destroy(pixelHumanoid);
        }
    }
}
