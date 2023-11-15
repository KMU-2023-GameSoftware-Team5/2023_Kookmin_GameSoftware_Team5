using data;
using battle;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.Events;
using UnityEngine.UIElements;

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

        /////////////////////////////////////////////////////////////////////////////////
        // 캐릭터 배치관련 

        /// <summary>
        /// 배치 컴포넌트 모음
        /// </summary>
        List<PlacementCharacter> placementUIs;
        /// <summary>
        /// 전체 CharacterLI
        /// </summary>
        List<SelectCharacter> characterUIs;
        /// <summary>
        /// 현재 선택된 CharacterLI. 좌측에 생성된 SelectedCharacter와는 관련이 없다.
        /// </summary>
        List<SelectCharacter> selectedCharacters;
        /// <summary>
        /// 플레이어가 배치한 캐릭터 저장
        /// </summary>
        JArray selectedCharacterSaveList;

        public UnityEvent initializePlaecmentEvent;

        /////////////////////////////////////////////////////////////////////////////////
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
        
        /// <summary>
        /// 캐릭터 선택 및 배치 게임오브젝트
        /// </summary>
        [SerializeField]GameObject selectCharacter;

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
            characterUIs = new List<SelectCharacter>();
            for (int i = 0; i < characters.Count; i++)
            {
                GameObject go = Instantiate(selectCharacter, characterInventoryGrid);
                Transform parentTransform = go.transform;
                go = go.transform.GetChild(0).gameObject;

                SelectCharacter sc = go.GetComponent<SelectCharacter>();
                sc.Initialize(characters[i], UIDragCanvas.transform);
                characterUIs.Add(sc);
            }

            // 저장된 배치 정보 가져오기
            placementUIs = new List<PlacementCharacter>();
            selectedCharacters = new List<SelectCharacter>();
            if (PlayerManager.Instance().selectedCharacters == null || PlayerManager.Instance().selectedCharacters.Count == 0) // 배치 프리셋 정보가 없으면 초기화
            {
                selectedCharacterSaveList = new JArray();
                PlayerManager.Instance().selectedCharacters.Add(selectedCharacterSaveList);
            }
            else // 있으면 0번째를 사용함. 0번째 인덱스는 가장 최근에 플레이어의 배치를 의미함
            {
                selectedCharacterSaveList = (JArray)PlayerManager.Instance().selectedCharacters[0];
                loadSelectedCharacterInfo(selectedCharacterSaveList);
            }
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
        /// 현재 배치된 캐릭터 보여주기
        /// </summary>
        /// <param name="character">현재 배치된 캐릭터</param>
        void createSelectedCharacterLI(PixelCharacter character)
        {
            GameObject go = Instantiate(selectedCharacterPrefab, selectedCharacterList);
            go = go.transform.GetChild(0).gameObject;
            go.GetComponent<SelectedCharacter>().Initialize(character);
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
            headName.Initialize(character);
            ret.headName = headName;

            return ret;
        }

        /*** 캐릭터 배치 및 배치 해제 ***/

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
        /// <summary>
        /// 좌표를 받아서 캐릭터 배치
        /// </summary>
        /// <param name="chracter">배치할 캐릭터</param>
        /// <param name="characterPosition">캐릭터 배치 위치</param>
        /// <returns>배치 성공여부</returns>
        public bool placeCharacter(SelectCharacter characterLI, Vector3 characterPosition)
        {
            if (placementUIs.Count + 1 <= PlayerManager.MAX_SELECTED_CHARACTER) 
            {
                // 자신이 배치한 캐릭터 정보 받기 
                PixelCharacter character = characterLI.character;
                character.worldPosition = characterPosition;

                // 캐릭터 배치 객체 생성
                PlacementCharacter ret = buildPixelHumanoidByPixelCharacter((PixelHumanoid)character);
                placementUIs.Add(ret);

                // 선택된 캐릭터 정보 저장
                createSelectedCharacterLI(character);
                characterLI.isPlaced = true;
                selectedCharacters.Add(characterLI);
                return true;
            }
            else
            {
                MyDeckFactory.Instance().displayInfoMessage($"{PlayerManager.MAX_SELECTED_CHARACTER}캐릭터 이상 배치할 수 없습니다.");
                return false;
            }
        }

        /// <summary>
        /// 캐릭터 배치해제
        /// </summary>
        /// <param name="character">배치해제할 캐릭터</param>
        public void unPlaceCharacter(PixelCharacter character)
        {
            // 배치된 캐릭터의 게임 오브젝트 삭제
            for (int i = placementUIs.Count - 1; i >= 0; i--)
            {
                if (placementUIs[i].compareCharacter(character))
                {
                    PlacementCharacter pm = placementUIs[i];
                    placementUIs.RemoveAt(i);
                    pm.unSelect();
                    break;
                }
            }
            // 배치된 캐릭터 정보 객체 삭제 
            for (int i = selectedCharacters.Count - 1; i >= 0; i--)
            {
                if (selectedCharacters[i].character.ID == character.ID)
                {
                    selectedCharacters[i].isPlaced = false;
                    selectedCharacters.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 배치된 캐릭터 초기화
        /// </summary>
        public void initializePlacement()
        {
            for(int i=placementUIs.Count - 1;i >= 0; i--)
            {
                placementUIs[i].unSelect();
                placementUIs.RemoveAt(i);
            }
            for (int i = selectedCharacters.Count - 1; i >= 0; i--)
            {
                selectedCharacters[i].isPlaced = false;
                selectedCharacters.RemoveAt(i);
            }
            initializePlaecmentEvent.Invoke();
        }

        /*** 배치된 캐릭터 정보 저장 및 불러오기 ***/

        /// <summary>
        /// JSON 형태로 배치된 캐릭터 배치정보 저장하기
        /// </summary>
        /// <returns>JArray로 배치된 캐릭터 배치정보</returns>
        public JArray saveSelectedCharacterInfo()
        {
            JArray ret = new JArray ();
            foreach(var characterLI in selectedCharacters)
            {
                PixelCharacter character = characterLI.character;
                JObject tmp = new JObject();
                tmp["id"] = character.ID;
                tmp["position"] = new JObject {
                    {"x", character.worldPosition.x },
                    {"y", character.worldPosition.y},
                    {"z", character.worldPosition.z},
                };
                ret.Add(tmp);
            }
            return ret;
        }

        /// <summary>
        /// JSON 형태로 저장해둔 캐릭터 배치정보 불러오기
        /// </summary>
        /// <param name="selecterCharacterInfo">JSON형태로 저장해둔 캐릭터 배치정보</param>
        public void loadSelectedCharacterInfo(JArray selecterCharacterInfo)
        {
            List<SelectCharacter> tmp = new List<SelectCharacter>();
            List<Vector3> tmpPosition = new List<Vector3>();

            for (int i = selecterCharacterInfo.Count - 1; i >= 0; i--) // 배치된 캐릭터 정보
            {
                bool tmpFlag = false;
                foreach (var character in characterUIs) // 현재 UI를 다 돈다
                {
                    if (character.character.ID == selecterCharacterInfo[i]["id"].ToString()) // 두 캐릭터 객체의 ID가 같으면 작동
                    {
                        tmpFlag = true;
                        tmp.Add(character);
                        tmpPosition.Add(
                            new Vector3(
                                (float)selecterCharacterInfo[i]["position"]["x"],
                                (float)selecterCharacterInfo[i]["position"]["y"],
                                (float)selecterCharacterInfo[i]["position"]["z"]
                            )
                        );
                        break;
                    }
                }
                if (tmpFlag)
                {
                    continue;
                }
                else
                {
                    // 캐릭터가 삭제되었다면 제거
                    selecterCharacterInfo.RemoveAt(i);
                }
            }
            for (int i = tmp.Count - 1; i >= 0; i--)
            {
                placeCharacter(tmp[i], tmpPosition[i]);
            }
        }

        public void save()
        {
            PlayerManager.Instance().selectedCharacters[0] = saveSelectedCharacterInfo();
            PlayerManager.save();
        }

    }
}
