using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
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
        public TmpCharacter[] characters;
        /// <summary>
        /// 현재 플레이어가 선택한 캐릭터의 집합
        /// </summary>
        public TmpCharacter[] selectCharacters;
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

        void Start()
        {
            // 임시 데이터 생성
            characters = new TmpCharacter[6];
            characters[0] = new TmpCharacter("blue", Color.blue);
            characters[1] = new TmpCharacter("magenta", Color.magenta);
            characters[2] = new TmpCharacter("cyan", Color.cyan);
            characters[3] = new TmpCharacter("yellow", Color.yellow);
            characters[4] = new TmpCharacter("red", Color.red);
            characters[5] = new TmpCharacter("green", Color.green);

            // 현재 보유중인 캐릭터 출력
            for (int i = 0; i < characters.Length; i++)
            {
                createCharacterInventoryPrefeb(i, characters[i]); 
            }

            // 캐릭터 선택 슬롯 생성
            selectCharacters = new TmpCharacter[5]; 
            selectors = new CharacterSelector[5];
            for(int i=0; i < selectors.Length; i++)
            {
                selectors[i] = createCharacterSelectorPrefeb(i);

            }
        }

        /// <summary>
        /// 플레이어가 현재 보유 중인 캐릭터 정보 UI 생성
        /// </summary>
        /// <param name="i">추후 제거 필요(미사용)</param>
        /// <param name="character">플레이어의 캐릭터 정보</param>
        void createCharacterInventoryPrefeb(int i, TmpCharacter character)
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
        public void openCharacterDetails(TmpCharacter character)
        {
            characterDetails.openCharacterDetails(character);
        }

        /// <summary>
        /// 캐릭터 선택 처리 메서드
        /// </summary>
        /// <param name="selectId">캐릭터가 몇번에 선택되었는지</param>
        /// <param name="character">선택된 캐릭터 정보</param>
        public void selectCharacter(int selectId, TmpCharacter character)
        {
            selectCharacters[selectId] = character;
            logSelectors();
        }

        /// <summary>
        /// 캐릭터 선택해제 이벤트
        /// </summary>
        /// <param name="selectId">선택해제될 캐릭터가 몇번 슬롯에 선택된 캐릭터인가</param>
        public void unSelectCharacter(int selectId)
        {
            selectCharacters[selectId] = null;
            //logSelectors();
        }

        /// <summary>
        /// 디버깅용 임시 로그 함수(추후제거)
        /// </summary>
        void logSelectors()
        {
            string ret = "nowSelected\n";
            for(int i = 0; i < selectCharacters.Length; i++)
            {
                if (selectCharacters[i] == null)
                {
                    ret += $"null\n";
                }
                else
                {
                    ret += $"{selectCharacters[i].getName()}";
                }
            }
            Debug.Log(ret);
        }
    }
}
