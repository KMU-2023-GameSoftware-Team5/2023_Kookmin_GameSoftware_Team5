using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    [CreateAssetMenu(fileName = "helpInfo", menuName = "helpInfo/helpInfo", order = 1)]
    public class HelpInfoScriptAble : ScriptableObject
    {
        /// <summary>
        /// 도움말의 제목입니다. 
        /// </summary>
        public string title;

        /// <summary>
        /// 도움말의 세부 내용입니다.
        /// </summary>
        [TextArea] public string description;

        /// <summary>
        /// 첨부사진이 필요한 경우 사용가능한 이미지입니다.
        /// </summary>
        public Sprite helpImg;
    }

}
