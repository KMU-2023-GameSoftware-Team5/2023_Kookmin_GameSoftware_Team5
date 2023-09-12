using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    /* TODO 
         * �̱������� ����� 
         * �� �Ѿ�� ����� �� �ְ� �ϱ� 
         * Character �߰� �� ���� �����ϱ� 
     */

    /* 
        * Character�� �����ϵ��� �����ϴ� Manager ��ü(�̱���)
        * �Ӽ�
            * CharacterInfoList : Character ����â���� ����ϴ� �׸���
            * CharInfoPrefab : Character ����â���� ĳ������ ������ �����ִ� ������Ʈ
            
            * CharSelectorList : ���� ���õ� ĳ����â���� ����ϴ� �׸���
            * CharSelectPrefab : ���� ���õ� ĳ������ ������ �����ִ� ������Ʈ
            
            * characters
            * selectors
            * nowSelectorId
           
        * �޼���
            * openCharacterList : ĳ���� ����â ���� 
            * chooseCharacter : ĳ���� ����â �ݱ�
     */

    private static CharacterManager Instance;
    public static CharacterManager instance
    {
        get { 
            if (Instance == null)
            {
                Instance = FindObjectOfType<CharacterManager>();
            }
            return Instance;
        }

    }

    public Transform CharacterInfoList;
    public GameObject CharInfoPrefab;

    public Transform CharSelectorList;
    public GameObject CharSelectPrefab;

    public TmpCharacter[] characters;
    public CharacterSelector[] selectors;
    public int nowSelectorId;

    // Start is called before the first frame update
    void Start()
    {
        // �׽�Ʈ�� ���� ������ ��ü ���� �ڵ� 
        characters = new TmpCharacter[5];
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = new TmpCharacter($"Character Name {i}");
            createCharPrefeb(i,characters[i]);
        }
        selectors = new CharacterSelector[3];
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
        MainSceneEvent.instance.OnClickCharacter();
    }

    public void chooseCharacter(int charId)
    {
        selectors[nowSelectorId].setCharInfo(characters[charId]);
        MainSceneEvent.instance.OnClickCharacterClose();
    }
}
