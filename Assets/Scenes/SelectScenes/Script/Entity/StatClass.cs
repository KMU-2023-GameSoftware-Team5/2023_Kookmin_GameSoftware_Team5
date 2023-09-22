using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
{
    /// <summary>
    /// 캐릭터와 아이템 등 스텟 정보를 모아 관리하는 객체
    /// </summary>
    // TODO: 스텟시스템 추가
    public class StatClass
    {
        public int hp;
        public int damage;
        public float walkSpeed;
        public float attackRange;
        public float attackDelay;
    }
}
