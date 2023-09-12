using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    /*
     * 현재 선택된 캐릭터의 정보를 보여주는 컴포넌트
     * 속성
        * selectId : 캐릭터 선택창에서 몇번인지 
        * CharacterInfo : 현재 선택된 캐릭터창에서 캐릭터가 선택되었을 경우 캐릭터 정보를 보여줌
        * tmpCharacter : 현재 선택된 캐릭터 정보
     * 메서드 
        * chooseCharacter : 캐릭터 선택기능
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
