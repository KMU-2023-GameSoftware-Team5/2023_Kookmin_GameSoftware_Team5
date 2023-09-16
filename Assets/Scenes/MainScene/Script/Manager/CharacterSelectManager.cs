using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    /* TODO 
         * �� �Ѿ�� ����� �� �ְ� �ϱ� 
         * Character �߰� �� ���� �����ϱ� 
         * ĳ���� �ߺ� ���� ����
     */

    /* 
        * Character�� �����ϵ��� �����ϴ� Manager ��ü(�̱���)
        * �Ӽ�
            * CharacterInfoList : Character ����â���� ����ϴ� �׸���
            * CharInfoPrefab : Character ����â���� ĳ������ ������ �����ִ� ������Ʈ
            
            * CharSelectorList : ���� ���õ� ĳ����â���� ����ϴ� �׸���
            * CharSelectPrefab : ���� ���õ� ĳ������ ������ �����ִ� ������Ʈ
            
            * characters : �÷��̾ �����ϰ� �ִ� ��� ĳ���͵�
            * selectors : ���� ���õ� ĳ���͵�
            * nowSelectorId, nowItemId
           
        * �޼���
            * openCharacterList : ĳ���� ����â ���� 
            * chooseCharacter : ĳ���� ����â �ݱ�
            * openSelectEquipItemEvent : ��� ������ ���� ��û
            * closeSelectEquipItemEvent : ���õ� �������� ����
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
        // �׽�Ʈ�� ���� ������ ��ü ���� �ڵ� 
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
