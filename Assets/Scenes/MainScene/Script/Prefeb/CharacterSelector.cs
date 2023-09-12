using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    /*
     * ���� ���õ� ĳ������ ������ �����ִ� ������Ʈ
     * �Ӽ�
        * selectId : ĳ���� ����â���� ������� 
        * CharacterInfo : ���� ���õ� ĳ����â���� ĳ���Ͱ� ���õǾ��� ��� ĳ���� ������ ������
        * tmpCharacter : ���� ���õ� ĳ���� ����
     * �޼��� 
        * chooseCharacter : ĳ���� ���ñ��
     */
    public int selectId;
    public GameObject CharacterInfoUI;
    public GameObject chooseCharcterUI;
    public TmpCharacter tmpCharacter;
    public TextMeshProUGUI CharacterDescription;

    void Start()
    {
        
    }

    public void setCharInfo(TmpCharacter character)
    {
        tmpCharacter = character;
    }

    // Update is called once per frame
    void Update()
    {
        if(tmpCharacter == null)
        {
            chooseCharcterUI.SetActive(true);
            CharacterInfoUI.SetActive(false);
        }
        else
        {
            chooseCharcterUI.SetActive(false) ;
            CharacterInfoUI.SetActive(true);
            CharacterDescription.text = tmpCharacter.ToString();
        }
    }
    
    public void chooseCharacter()
    {
        CharacterManager.instance.openCharacterList(selectId);
    }
}
