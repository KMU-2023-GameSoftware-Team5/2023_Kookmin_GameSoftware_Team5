using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;
using System;

namespace deck
{
    /// <summary>
    /// PixelCharacter의 구현체. PixelHumanoid와 호환
    /// </summary>
    public class PixelHumanoid : PixelCharacter
    {
        /// <summary>
        /// 임시로 모아놓은 캐릭터 닉네임들
        /// </summary>
        static string[] characterNickNames = {
                "blue",
                "magenta",
                "yellow",
                "cyan",
                "red",
                "green",
            };

        /// <summary>
        /// 픽셀 캐릭터 생성자
        /// </summary>
        /// <param name="characterName">픽셀 캐릭터의 이름(유니크한)</param>
        public PixelHumanoid(string characterName, CommonStats characterStat)
        {
            this.characterName = characterName;
            characterNickName = nickNameMaker();

            // 캐릭터 초기 위치 설정
            worldPosition = characterInitPosition();

            // 캐릭터 설정
            this.characterStat = characterStat;
            
            // 아이템 인벤토리
            Inventory = new EquipItem[EquipItemManager.MAX_INVENTORY_SIZE];
        }

        /// <summary>
        /// 임시로 닉네임 만드는 메서드
        /// </summary>
        /// <returns>캐릭터 닉네임</returns>
        string nickNameMaker()
        {
            System.Random random = new System.Random();

            string ret = characterNickNames[random.Next(0, characterNickNames.Length)];
            return ret;
        }

        /// <summary>
        /// TODO : 캐릭터 초기위치 설정
        /// </summary>
        /// <returns>캐릭터의 초기 worldspace 좌표</returns>
        Vector3 characterInitPosition()
        {
            Vector3 ret = new Vector3(UnityEngine.Random.Range(-6, -0), UnityEngine.Random.Range(3, -4), 0); 
            return ret;
        }
    }
}
