using data;
using battle;
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
        public static CharacterSelectManager Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterSelectManager>();
            }
            return instance;             
        }

        /// <summary>
        /// 현재 플레이어가 가지고 있는 캐릭터의 집합
        /// </summary>
        public List<PixelCharacter> characters;
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
        GameObject selectUICanvas;

        [Header("Character List")]
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

        [Header("Character Selector UI")]
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

        [Header("Character Details")]
        /// <summary>
        /// 캐릭터 세부 정보창 UI
        /// </summary>
        [SerializeField]
        CharacterDetails characterDetails;

        /// <summary>
        /// 배치 UI 배열
        /// /summary>
        PlacementCharacter[] placementUIs;

        /// <summary>
        /// 캐릭터 배치 모드. TODO
        /// </summary>
        public bool isPlacementMode { get; private set; }

        [Header("Character Placement")]
        /// <summary>
        /// 임시속성 - 캐릭터 배치 오브젝트의 parent canvas
        /// </summary>
        public Transform placementCanvas;

        /// <summary>
        /// placement Character(캐릭터 배치 오브젝트) 프리펩
        /// </summary>
        [SerializeField] GameObject placementCharacterPrefab;

        [Header("Character Placement Head Bar")]
        /// <summary>
        /// Placement - 캐릭터 헤드 네임이 위치할 캔버스
        /// </summary>
        public Transform characterHeadNameCanvas;
        /// <summary>
        /// Placement - 배치시 캐릭터 위에 이름 달아줄 프리펩
        /// </summary>
        [SerializeField] GameObject characterHeadNamePrefab;

        battle.PixelCharacter[] battlePixelCharacters;

        void Start()
        {
            isPlacementMode = false;

            // 플레이어매니저에게서 보유 캐릭터 받아오기
            characters = PlayerManager.Instance().playerCharacters;


            // 더미데이터 생성
            if(characters.Count == 0)
            {
                string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
                System.Random random = new System.Random();
                for (int i = 0; i < 7; i++)
                {
                    PlayerManager.Instance().addCharacterByName(characterNames[random.Next(0, characterNames.Length)]);
                }
            }

            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Count; i++)
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
            battlePixelCharacters = new battle.PixelCharacter[5];
        }

        /// <summary>
        /// 플레이어가 현재 보유 중인 캐릭터 정보 UI 생성
        /// </summary>
        /// <param name="i">TODO - 추후 제거 필요(미사용)</param>
        /// <param name="character">플레이어의 캐릭터 정보</param>
        void createCharacterInventoryPrefeb(int i, PixelCharacter character)
        {
            GameObject newPrefab = Instantiate(characterInventoryItemPrefeb, characterInventoryGrid);
            newPrefab.GetComponent<CharacterListItem>().Initialize(character, selectUICanvas.transform, characterInventoryGrid.transform);
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
            placementUIs[selectId] = buildPixelHumanoidByPixelCharacter(
                (PixelHumanoid)selectCharacters[selectId]
            );
            battlePixelCharacters[selectId] =  placementUIs[selectId].GetComponent<battle.PixelCharacter>();
        }

        /// <summary>
        /// 캐릭터 선택해제 이벤트
        /// </summary>
        /// <param name="selectId">선택해제될 캐릭터가 몇번 슬롯에 선택된 캐릭터인가</param>
        public void unSelectCharacter(int selectId)
        {
            selectCharacters[selectId] = null;
            battlePixelCharacters[selectId] = null;
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

        /// <summary>
        /// 캐릭터 배치 모드로 만들기. TODO
        /// </summary>
        public void togglePlacementMode()
        {
            isPlacementMode = isPlacementMode ? false : true;
            selectUICanvas.SetActive(!isPlacementMode);
            foreach(PlacementCharacter ui in placementUIs)
            {
                if(ui != null)
                {
                    ui.dragMode = isPlacementMode;
                }
            }
        }

        /// <summary>
        /// 배치할 캐릭터를 생성하는 메서드
        /// </summary>
        /// <param name="character">캐릭터 셀렉트 매니저가 가진 캐릭터 정보</param>
        /// <returns>캐릭터셀렉트매니저가 관리할 수 있는 캐릭터 배치 컴포넌트</returns>
        public PlacementCharacter buildPixelHumanoidByPixelCharacter(deck.PixelHumanoid character)
        {
            // Instantiate pixel humanoid
            Vector3 worldPosition = character.worldPosition;
            GameObject characterGo = Instantiate(placementCharacterPrefab, Vector3.zero, Quaternion.identity, placementCanvas);
            characterGo.transform.position = worldPosition;

            // build battle.PixelHumaniod
            battle.PixelHumanoid battlPixelHumanoid = characterGo.GetComponent<battle.PixelHumanoid>();
            battlPixelHumanoid.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            battlPixelHumanoid.builder.SpriteLibrary = battlPixelHumanoid.spriteLibrary;
            PixelHumanoidData data = MyDeckFactory.Instance().getPixelHumanoidData(character.characterName);
            data.SetOutToBuilder(battlPixelHumanoid.builder);
            battlPixelHumanoid.builder.Rebuild();
            battlPixelHumanoid.Initilize(data);

            // PlacementCharacter build
            PlacementCharacter ret = characterGo.GetComponent<PlacementCharacter>();
            ret.Initialize(character);

            // Instantiate head Bar
            GameObject headBarGo = Instantiate(characterHeadNamePrefab, Vector3.zero, Quaternion.identity, characterHeadNameCanvas);

            // head bar setting
            PixelCharacterHeadBar headBar = headBarGo.GetComponent<PixelCharacterHeadBar>();
            headBar.Initialize(battlPixelHumanoid);
            battlPixelHumanoid.headBar = headBar;

            // head name setting
            PlacementCharacterHeadName headName = headBarGo.GetComponent<PlacementCharacterHeadName>();
            headName.Initialize(character.characterNickName);
            ret.headName = headName;

            return ret;
        }

        public battle.PixelCharacter[] battleStart()
        {
            foreach(PlacementCharacter target in placementUIs)
            {
                if(target != null)
                {
                    target.battleStart();
                }
            }

            return battlePixelCharacters;
        }
    }
}
