using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharInfoListItem : MonoBehaviour
{
    /*
        * Character 선택창에서 캐릭터의 정보를 보여주는 컴포넌트
        * TODO
            * UI
     */

    public TmpCharacter charInfo;
    public TextMeshProUGUI tmpCharInfo;
    public int charId;

    void Start()
    {
        
    }

    public void setCharInfo(int i, TmpCharacter obj)
    {
        charInfo = obj;
        charId = i;
    }

    public void chooseCharacter()
    {
        CharacterManager.instance.chooseCharacter(charId);
    }

    // Update is called once per frame
    void Update()
    {
        tmpCharInfo.text = charInfo.ToString();
    }

    
}
