using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 캐릭터 선택 관리 객체
    /// </summary>
    /// <remarks>
    /// 캐릭터 선택 씬에서 캐릭터 관련 업무를 처리함. 추후 캐릭터 전반적인 관리로 확장할지는 논의 필요
    /// </remarks>
    public class CharacterSelectManager : MonoBehaviour
    {
        private static CharacterSelectManager instance;
        public static CharacterSelectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterSelectManager>();
                }
                return instance;
            }
             
        }

        /// <summary>
        /// 현재 플레이어가 가지고 있는 캐릭터의 집합
        /// </summary>
        public PixelCharacter[] characters;
        /// <summary>
        /// 현재 플레이어가 선택한 캐릭터의 집합
        /// </summary>
        public PixelCharacter[] selectCharacters;
        /// <summary>
        /// 캐릭터 선택 슬롯에 대한 배열
        /// </summary>
        public CharacterSelector[] selectors;
        
        /// <summary>
        /// 캐릭터 선택 이벤트 처리를 위한 임시변수
        /// </summary>
        public int nowSelectorId;
        
        /// <summary>
        /// Drag 이벤트 처리를 위한 canvas레퍼런스
        /// </summary>
        [SerializeField]
        GameObject canvas;

        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터들을 보여주는 리스트의 위치
        /// </summary>
        [SerializeField]
        Transform characterInventoryGrid;
        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터정보 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterInventoryItemPrefeb; 

        /// <summary>
        /// 캐릭터 선택창 위치
        /// </summary>
        [SerializeField]
        Transform characterSelectorGrid;
        /// <summary>
        /// 캐릭터 선택 슬롯에 대한 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterSelectorPrefeb;

        /// <summary>
        /// 캐릭터 세부 정보창 UI
        /// </summary>
        [SerializeField]
        CharacterDetails characterDetails;

        /// <summary>
        /// 배치 UI 배열
        /// /summary>
        PlacementCharacter[] placementUIs;

        void Start()
        {
            // 임시 데이터 생성
            characters = new PixelCharacter[6];
            
            characters[0] = MyDeckFactory.Instance().buildPixelCharacter("blue");
            characters[1] = MyDeckFactory.Instance().buildPixelCharacter("magenta");
            characters[2] = MyDeckFactory.Instance().buildPixelCharacter("cyan");
            characters[3] = MyDeckFactory.Instance().buildPixelCharacter("yellow");
            characters[4] = MyDeckFactory.Instance().buildPixelCharacter("red");
            characters[5] = MyDeckFactory.Instance().buildPixelCharacter("green");

            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Length; i++)
            {
                createCharacterInventoryPrefeb(i, characters[i]); 
            }

            // 캐릭터 선택 슬롯 생성
            selectCharacters = new PixelCharacter[5]; 
            selectors = new CharacterSelector[5];
            for(int i=0; i < selectors.Length; i++)
            {
                selectors[i] = createCharacterSelectorPrefeb(i);

            }

            placementUIs = new PlacementCharacter[5];
        }

        /// <summary>
        /// 플레이어가 현재 보유 중인 캐릭터 정보 UI 생성
        /// </summary>
        /// <param name="i">추후 제거 필요(미사용)</param>
        /// <param name="character">플레이어의 캐릭터 정보</param>
        void createCharacterInventoryPrefeb(int i, PixelCharacter character)
        {
            GameObject newPrefab = Instantiate(characterInventoryItemPrefeb, characterInventoryGrid);
            newPrefab.GetComponent<CharacterListItem>().Initialize(character, canvas.transform, characterInventoryGrid.transform);
        }

        /// <summary>
        /// 캐릭터 선택 슬롯의 위치
        /// </summary>
        /// <param name="selectId">캐릭터 선택 슬롯의 고유 ID</param>
        /// <returns></returns>
        CharacterSelector createCharacterSelectorPrefeb(int selectId)
        {
            GameObject newPrefab = Instantiate(characterSelectorPrefeb, characterSelectorGrid);
            CharacterSelector selector = newPrefab.GetComponent<CharacterSelector>();
            selector.Initialize(selectId);
            return selector; 
        }

        /// <summary>
        /// 캐릭터 세부 정보창 열기
        /// </summary>
        /// <param name="character">세부 정보창을 열어야하는 캐릭터</param>
        public void openCharacterDetails(PixelCharacter character)
        {
            characterDetails.openCharacterDetails(character);
        }

        /// <summary>
        /// 캐릭터 선택 처리 메서드
        /// </summary>
        /// <param name="selectId">캐릭터가 몇번에 선택되었는지</param>
        /// <param name="character">선택된 캐릭터 정보</param>
        public void selectCharacter(int selectId, PixelCharacter character)
        {
            selectCharacters[selectId] = character;
            placementUIs[selectId] = MyDeckFactory.Instance().buildPixelHumanoidByPixelCharacter(
                (PixelHumanoid)selectCharacters[selectId]
            );
        }

        /// <summary>
        /// 캐릭터 선택해제 이벤트
        /// </summary>
        /// <param name="selectId">선택해제될 캐릭터가 몇번 슬롯에 선택된 캐릭터인가</param>
        public void unSelectCharacter(int selectId)
        {
            selectCharacters[selectId] = null;
            Debug.Log(placementUIs[selectId]);
            placementUIs[selectId].unSelect();
            placementUIs[selectId] = null;
            //logSelectors();
        }

        /// <summary>
        /// 캐릭터에게 아이템 장착 이벤트
        /// </summary>
        /// <param name="character">아이템을 장착할 캐릭터</param>
        /// <param name="equipId">캐릭터가 몇번 인벤토리에 아이템을 장착할 것인지</param>
        /// <param name="item">장착할 아이템</param>
        public bool equip(PixelCharacter character, int equipId, EquipItem item)
        {
            return character.equip(equipId, item);
        }

        /// <summary>
        /// 캐릭터 아이템 장착 해제 이벤트
        /// </summary>
        /// <param name="character">아이템을 해제할 캐릭터</param>
        /// <param name="equipId">해제될 아이템의 인벤토리상 위치</param>
        public bool unEquip(PixelCharacter character, int equipId)
        {
            return character.unEquip(equipId);
        }

    }
}
