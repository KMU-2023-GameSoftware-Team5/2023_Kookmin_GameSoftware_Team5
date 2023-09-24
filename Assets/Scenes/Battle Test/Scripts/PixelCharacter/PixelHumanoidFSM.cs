using System;
using UnityEngine;

namespace lee
{
    public partial class PixelHumanoid
    {
        public enum EState
        {
            Waiting,
            Searching, // 바라보고 있는 방향으로 이동하면서 범위 안의 적을 찾는다. 
            Chasing, // 공격 타깃에 공격 범위 밖에 있어서 따라간다. 
            MeleeAttacking,
            RangedAttacking,
            Delaying,
            Dead,
            None, 
        }

        public partial class State
        {
            public State() {}

            // 이 부분을 상속으로 구현하면 상태 하나마다 클래스 하나가 되는데, 코드가 길어지는 것이 싫다. 
            // 그래서 대리자로 했다. 
            // 대리자 구현은 PixelHumnaoid.StateFactory에서 확인할 수 있다.
            public Action<PixelHumanoid> Start;
            public Func<PixelHumanoid, EState> Update;
            public Action<PixelHumanoid> End;

            // 추가적인 멤버 변수가 필요한 경우는 상속으로 추가한다. 
        }

        public class StateSet
        {
            public State Waiting;
            public State Searching;
            public State Chasing;

            // TODO: DefaultAttacking으로 합치기
            public State MeleeAttacking;
            public State RangedAttacking;
            public State Delaying;
            public State Dead;

            // maps between EState and State
            public State Get(EState state)
            {
                switch (state)
                {
                    case EState.Waiting:
                        return Waiting;
                    case EState.Searching:
                        return Searching;
                    case EState.Chasing:
                        return Chasing;
                    case EState.Dead:
                        return Dead;
                    case EState.MeleeAttacking:
                        return MeleeAttacking;
                    case EState.RangedAttacking:
                        return RangedAttacking;
                    case EState.Delaying:
                        return Delaying;
                    default:
                        Debug.LogError("FATAL: PixelHumanoid.State.Get() switch defatul case: INVALID MAPPING");
                        return null;
                }
            }

            public static StateSet CreateTestStateSet0()
            {
                return new StateSet()
                {
                    Waiting = StateFactory.GetWaitingState(), 
                    Searching = StateFactory.GetSearchingState(),
                    Chasing = StateFactory.GetChasingState(), 
                    MeleeAttacking = StateFactory.GetMeleeAttackingState(), 
                    RangedAttacking = StateFactory.GetRangedAttackingState(),
                    Delaying = StateFactory.GetDelayingState(),
                    Dead = StateFactory.GetDeadState()
                };
            }
        }

        public class FSM
        {
            // null FSM
            public FSM() 
            { 
                m_currentEState = EState.None;
                m_transitionToDead = false;
                m_transitionToSearching = false;
            }

            public FSM(StateSet stateSet) : this()
            {
                m_stateSet = stateSet;
                m_currentState = m_stateSet.Get(EState.Waiting);
                m_currentEState = EState.Waiting;
            }

            public FSM(StateSet stateSet, EState initialState) : this()
            {
                m_stateSet = stateSet;
                m_currentState = m_stateSet.Get(initialState);
                m_currentEState = initialState;
                m_transitionToDead = false;
            }

            public void Update(PixelHumanoid owner)
            {
                if (m_currentState == null)
                    return;

                if (m_currentState.Update == null)
                    return;

                EState newState = EState.Dead;
                if (!m_transitionToDead)
                    newState = m_currentState.Update(owner);
                if (m_transitionToSearching)
                    newState = EState.Searching;

                if (newState != EState.None)
                {
                    if (m_currentState.End != null)
                        m_currentState.End(owner);

                    m_currentEState = newState;
                    m_currentState = m_stateSet.Get(newState);

                    if (m_currentState.Start != null)
                        m_currentState.Start(owner);
                }
            }

            private bool m_transitionToDead;
            public void SetTransitionToDead(bool yesOrNo)  // from any state
            {
                m_transitionToDead = yesOrNo;
            }

            private bool m_transitionToSearching;
            public void SetTransitionToSearch(bool yesOrNo)    // from any state
            {
                m_transitionToSearching = yesOrNo;
            }

            private EState m_currentEState;
            public EState GetCurrentState() { return m_currentEState; }

            private State m_currentState;
            private StateSet m_stateSet;
        }
    }
}
