using jslee;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharInfoListItem : MonoBehaviour
{
    /*
        * Character ����â���� ĳ������ ������ �����ִ� ������Ʈ
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
        CharacterSelectManager.Instance.chooseCharacter(charId);
    }

    // Update is called once per frame
    void Update()
    {
        tmpCharInfo.text = charInfo.ToString();
    }
}
