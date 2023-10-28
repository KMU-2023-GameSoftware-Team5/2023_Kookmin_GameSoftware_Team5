using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck{
    /// <summary>
    /// 플레이어의 상태 저장 불러오기에 대한 추상 클래스
    /// </summary>
    public abstract class SaveLoadManager
    {
        /// <summary>
        /// 게임 중 플레이어의 상태를 저장하는 메서드
        /// </summary>
        /// <param name="playerManager">저장할 플레이어 상태 객체</param>
        /// <param name="path">저장할 경로 </param>
        public abstract void save(PlayerManager playerManager, string path="PlayerManager");

        /// <summary>
        /// 저장한 플레이어의 상태를 불러오는 메서드
        /// </summary>
        /// <param name="playerManager">플레이어 상태 객체</param>
        /// <param name="path">저장한 경로 </param>
        public abstract void load(PlayerManager playerManager, string path = "PlayerManager");

        /// <summary>
        /// Save 파일 삭제하기
        /// </summary>
        public abstract void delete(PlayerManager playerManager, string path = "PlayerManager");
    }
}
