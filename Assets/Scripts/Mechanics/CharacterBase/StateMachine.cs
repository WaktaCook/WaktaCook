using System.Collections.Generic;

namespace WaktaCook.Mechanics.Characters
{
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<string, BaseState> states = new Dictionary<string, BaseState>();

        public void AddState(string stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
                states.Add(stateName, state);
            else
                states[stateName] = state;
        }

        public BaseState GetState(string stateName)
        {
            if (states.TryGetValue(stateName, out BaseState state))
                return state;
#if ENABLE_LOG
            else
            {
                UnityEngine.Debug.LogWarning($"StateMachine.GetState({state}) 존재하지 않는 StateName입니다.");
            }
#endif
            return null;
        }

        public void RemoveState(string removeStateName)
        {
            if (states.ContainsKey(removeStateName))
                states.Remove(removeStateName);
#if ENABLE_LOG
            else
            {
                UnityEngine.Debug.LogWarning($"StateMachine.RemoveState({removeStateName}) 존재하지 않는 StateName입니다.");
            }
#endif
        }

        public void ChangeState(string nextStateName)
        {
            if (states.TryGetValue(nextStateName, out BaseState nextState))
            {
                CurrentState?.Exit();
                CurrentState = nextState;
                CurrentState?.Enter();
            }
#if ENABLE_LOG
            else
            {
                UnityEngine.Debug.LogWarning($"StateMachine.ChangeState({nextStateName}) 존재하지 않는 StateName입니다.");
            }
#endif
        }

        public void UpdateState()
        {
            CurrentState?.LogicUpdate();
        }

        public void FixedUpdateState()
        {
            CurrentState?.PhysicsUpdate();
        }

        public void Initialize()
        {
            foreach (var state in states.Values)
            {
                state.Initailize();
            }
        }
    }
}
