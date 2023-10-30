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
        /// 캐릭터 선택 이벤트 처리를 위한 임시변수
        /// </summary>
        public int nowSelectorId;
        
        /// <summary>
        /// Drag 이벤트 처리를 위한 canvas레퍼런스
        /// </summary>
        [SerializeField]
        GameObject UIDragCanvas;
        /// <summary>
        /// Select UI
        /// </summary>
        [SerializeField]GameObject selectCanvas;

        [Header("Character List")]
        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터들을 보여주는 리스트의 위치
        /// </summary>
        [SerializeField]
        Transform characterInventoryGrid;

        [Header("Selected Character UI")]
        /// <summary>
        /// 선택된 캐릭터 리스트
        /// </summary>
        [SerializeField]
        Transform selectedCharacterList;
        /// <summary>
        /// 선택된 캐릭터 프리펩
        /// </summary>
        [SerializeField]
        GameObject selectedCharacterPrefab;

        [Header("Character Details")]
        /// <summary>
        /// 캐릭터 세부 정보창 UI
        /// </summary>
        [SerializeField]
        CharacterDetails characterDetails;

        /////////////////////////////////////////////////////////////////////////////////
        // 캐릭터 배치관련 

        /// <summary>
        /// 배치 컴포넌트 모음
        /// </summary>
        List<PlacementCharacter> placementUIs;
        /// <summary>
        /// 전체 CharacterLI
        /// </summary>
        List<CharacterListItem> characterUIs;
        /// <summary>
        /// 현재 선택된 CharacterLI. 좌측에 뜨는 SelectedCharacter와는 관련이 없다.
        /// </summary>
        List<CharacterListItem> selectedCharacters;

        /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 캐릭터 배치 모드. TODO
        /// </summary>
        public bool isPlacementMode { get; private set; }

        [Header("Character Placement")]
        /// <summary>
        /// 임시속성 - 캐릭터 배치 오브젝트의 parent canvas
        /// </summary>
        [SerializeField] Transform placementCanvas;

        /// <summary>
        /// placement Character(캐릭터 배치 오브젝트) 프리펩
        /// </summary>
        [SerializeField] GameObject placementCharacterPrefab;

        [Header("Character Placement Head Bar")]
        /// <summary>
        /// Placement - 캐릭터 헤드 네임이 위치할 캔버스
        /// </summary>
        [SerializeField] Transform characterHeadNameCanvas;
        /// <summary>
        /// Placement - 배치시 캐릭터 위에 이름 달아줄 프리펩
        /// </summary>
        [SerializeField] GameObject characterHeadNamePrefab;

        /// <summary>
        /// 세이브 로드 매니저
        /// </summary>
        SaveLoadManager saveLoadManager;
        
        void loadPlacementInfo()
        {
            // TODO 저장된 배치 정보 가져오기
            placementUIs = new List<PlacementCharacter>();
            selectedCharacters = new List<CharacterListItem>();
        }

        /// <summary>
        /// new characterLI for select scene
        /// </summary>
        [SerializeField]GameObject characterListItemSlot;

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
                for (int i = 0; i < 15; i++)
                {
                    PlayerManager.Instance().addCharacterByName(characterNames[random.Next(0, characterNames.Length)]);
                }
            }

            // 현재 보유중인 캐릭터 출력 - 배치가능한 사양으로
            characterUIs = new List< CharacterListItem >();
            for (int i = 0; i < characters.Count; i++)
            {
                GameObject go = Instantiate(characterListItemSlot, characterInventoryGrid);
                Transform parentTransform = go.transform;
                go = go.transform.GetChild(0).gameObject;
                
                go.GetComponent<LightCharacterListItem>().Initialize(characters[i], UIDragCanvas.transform, parentTransform);
                characterUIs.Add(go.GetComponent<CharacterListItem>());
                /*
                GameObject go=  MyDeckFactory.Instance().createCharacterInventoryPrefab(characters[i], characterInventoryGrid, false); 
                go.GetComponent<LightCharacterListItem>().Initialize(characters[i], UIDragCanvas.transform, characterInventoryGrid.transform);
                */
            }

            // TODO
            loadPlacementInfo();
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
        /// PlacementCharacter 버튼을 누르면 실행됨. 캐릭터 배치 모드로 만들기
        /// </summary>
        public void togglePlacementMode()
        {
            isPlacementMode = isPlacementMode ? false : true;
            selectCanvas.SetActive(!isPlacementMode);
            foreach(PlacementCharacter ui in placementUIs)
            {
                if(ui != null)
                {
                    ui.dragMode = isPlacementMode;
                }
            }
        }

        /// <summary>
        /// 좌표를 받아서 캐릭터 배치
        /// </summary>
        /// <param name="chracter">배치할 캐릭터</param>
        /// <param name="characterPosition">캐릭터 배치 위치</param>
        /// <returns>배치 성공여부</returns>
        public bool placeCharacter(CharacterListItem characterLI, Vector3 characterPosition)
        {
            if(placementUIs.Count +1 <= 5) // TODO - 상수화. 최대 배치가능 캐릭터 수 이하면 허용
            {
                PixelCharacter character = characterLI.getCharacter();
                character.worldPosition = characterPosition;
                PlacementCharacter ret =  buildPixelHumanoidByPixelCharacter((PixelHumanoid)character);
                placementUIs.Add(ret);
                createSelectedCharacterLI(character);
                characterLI.isPlaced = true;
                selectedCharacters.Add(characterLI);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void unPlaceCharacter(PixelCharacter character)
        {
            for(int i = placementUIs.Count - 1; i >= 0; i--)
            {
                if (placementUIs[i].compareCharacter(character))
                {
                    PlacementCharacter pm = placementUIs[i];
                    placementUIs.RemoveAt(i);
                    pm.unSelect();
                }
            }
            foreach(CharacterListItem characterListItem in selectedCharacters)
            {
                if (characterListItem.compareCharacter(character))
                {
                    characterListItem.isPlaced = false;
                    break;
                }
            }
        }

        /// <summary>
        /// 현재 배치된 캐릭터 보여주기
        /// </summary>
        /// <param name="character">현재 배치된 캐릭터</param>
        void createSelectedCharacterLI(PixelCharacter character)
        {
            GameObject go = Instantiate(selectedCharacterPrefab, selectedCharacterList);
            go.GetComponent<LightCharacterListItem>().Initialize(character);
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
        
        /*
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
        */

        /// <summary>
        /// 배틀씬으로 보내줌
        /// </summary>
        /// <returns></returns>
        public List<battle.PixelCharacter> battleStart()
        {
            List<battle.PixelCharacter> ret = new List<battle.PixelCharacter> ();
            foreach(PlacementCharacter target in placementUIs)
            {
                ret.Add(target.gameObject.GetComponent<battle.PixelCharacter>());
                target.battleStart();
            }
            return ret;
        }

        public void save()
        {
            PlayerManager.save();
        }

    }
}
