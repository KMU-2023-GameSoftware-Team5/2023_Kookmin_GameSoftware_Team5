using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    /* TODO 
         * 씬 넘어가도 사용할 수 있게 하기 
         * Character 추가 및 제거 구현하기 
         * 캐릭터 중복 선택 개선
     */

    /* 
        * Character를 선택하도록 지원하는 Manager 객체(싱글톤)
        * 속성
            * CharacterInfoList : Character 선택창에서 사용하는 그리드
            * CharInfoPrefab : Character 선택창에서 캐릭터의 정보를 보여주는 컴포넌트
            
            * CharSelectorList : 현재 선택된 캐릭터창에서 사용하는 그리드
            * CharSelectPrefab : 현재 선택된 캐릭터의 정보를 보여주는 컴포넌트
            
            * characters : 플레이어가 보유하고 있는 모든 캐릭터들
            * selectors : 현재 선택된 캐릭터들
            * nowSelectorId, nowItemId
           
        * 메서드
            * openCharacterList : 캐릭터 선택창 열기 
            * chooseCharacter : 캐릭터 선택창 닫기
            * openSelectEquipItemEvent : 장비 아이템 선택 요청
            * closeSelectEquipItemEvent : 선택된 장비아이템 장착
     */

    private static CharacterSelectManager instance;
    public static CharacterSelectManager Instance
    {
        get { 
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterSelectManager>();
            }
            return instance;
        }

    }

    public Transform CharacterInfoList;
    public GameObject CharInfoPrefab;

    public Transform CharSelectorList;
    public GameObject CharSelectPrefab;

    public TmpCharacter[] characters;
    public CharacterSelector[] selectors;
    public int nowSelectorId, nowItemId;

    // Start is called before the first frame update
    void Start()
    {
        // 테스트를 위한 아이템 객체 생성 코드 
        characters = new TmpCharacter[10];
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = new TmpCharacter($"Character Name {i}");
            createCharPrefeb(i,characters[i]);
        }
        selectors = new CharacterSelector[5];
        for (int i = 0;i < selectors.Length; i++)
        {
            GameObject newPrefab = Instantiate(CharSelectPrefab, CharSelectorList);
            selectors[i] = newPrefab.GetComponent<CharacterSelector>();
            selectors[i].selectId = i;
        }
    }

    void createCharPrefeb(int i, TmpCharacter character)
    {
        GameObject newPrefab = Instantiate(CharInfoPrefab, CharacterInfoList);
        newPrefab.GetComponent<CharInfoListItem>().setCharInfo(i, character);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openCharacterList(int selectorId)
    {
        nowSelectorId = selectorId;
        MainSceneEvent.Instance.OnClickCharacter();
    }

    public void chooseCharacter(int charId)
    {
        selectors[nowSelectorId].setCharInfo(characters[charId]);
        MainSceneEvent.Instance.OnClickCharacterClose();
    }

    public void openSelectEquipItemEvent(int charId, int itemId)
    {
        Debug.Log("2. openSelectItem Event By Character Manager");
        nowSelectorId = charId;
        nowItemId = itemId;
        EquipItemManager.Instance.openEquipItemForSelectEvent();
    }

    public void closeSelectEquipItemEvent(EquipItem item)
    {
        selectors[nowSelectorId].equip(nowItemId, item);
    }
}
