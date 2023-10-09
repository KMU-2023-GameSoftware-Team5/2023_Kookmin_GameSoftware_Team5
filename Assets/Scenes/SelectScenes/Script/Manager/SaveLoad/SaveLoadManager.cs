using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck{
    /// <summary>
    /// 플레이어의 상태 저장 불러오기에 대한 추상 클래스
    /// </summary>
    public abstract class SaveLoadManager : MonoBehaviour
    {



        /// <summary>
        /// 게임 중 플레이어의 상태를 저장하는 메서드
        /// </summary>
        public abstract void save();

        /// <summary>
        /// 게임 시작시 플레이어의 상태를 불러오는 메서드
        /// </summary>
        public abstract void load();

        /// <summary>
        /// 삭제하기
        /// </summary>
        public abstract void delete();
    }
}
