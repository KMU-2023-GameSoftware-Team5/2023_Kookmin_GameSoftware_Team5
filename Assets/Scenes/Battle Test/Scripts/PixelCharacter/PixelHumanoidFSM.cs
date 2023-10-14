using System;
using Unity.VisualScripting;
using UnityEngine;

namespace battle
{
    public partial class PixelHumanoid
    {
        public enum EState
        {
            Waiting,
            Searching, 
            Chasing,
            MeleeAttacking,
            RangedAttacking,
            Delaying,
            Skill,
            Dead,
            None,
        }

        public partial class State
        {
            public State() { }

            public Action<PixelHumanoid> OnEnter;
            public Func<PixelHumanoid, EState> OnUpdate;
            public Action<PixelHumanoid> OnEnd;
        }

        public abstract partial class ExtendedState : State
        {
            public ExtendedState() 
            {
                OnEnter = onEnter;
                OnUpdate = onUpdate;
                OnEnd = onEnd;
            }

            protected virtual void onEnter(PixelHumanoid owner) { }
            protected virtual EState onUpdate(PixelHumanoid owner) { return EState.None; }
            protected virtual void onEnd(PixelHumanoid owner) { }
        }

        public class StateSet
        {
            public State Waiting;
            public State Searching;
            public State Chasing;

            public State MeleeAttacking;
            public State RangedAttacking;
            public State Delaying;
            public State Dead;
            public State Skill;

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
                    case EState.Skill:
                        return Skill;
                    default:
                        Debug.LogError("FATAL: PixelHumanoid.State.Get() switch defatul case: INVALID MAPPING");
                        return null;
                }
            }

            public static StateSet StateSetWithSkill(ESkill skill)
            {
                StateFactory stateFactory = StateFactory.Instance();

                return new StateSet()
                {
                    Waiting = stateFactory.GetWaitingState(),
                    Searching = stateFactory.GetSearchingState(),
                    Chasing = stateFactory.GetChasingState(),
                    MeleeAttacking = stateFactory.GetMeleeAttackingState(),
                    RangedAttacking = stateFactory.GetRangedAttackingState(),
                    Delaying = stateFactory.GetDelayingState(),
                    Dead = stateFactory.GetDeadState(),
                    Skill = SkillFactory.Instance().GetSkill(skill)
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

                if (m_currentState.OnUpdate == null)
                    return;

                EState newState = EState.Dead;
                if (!m_transitionToDead)
                    newState = m_currentState.OnUpdate(owner);
                if (m_transitionToSearching)
                    newState = EState.Searching;

                if (newState != EState.None)
                {
                    if (m_currentState.OnEnd != null)
                        m_currentState.OnEnd(owner);

                    m_currentEState = newState;
                    m_currentState = m_stateSet.Get(newState);

                    if (m_currentState.OnEnter != null)
                        m_currentState.OnEnter(owner);
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
