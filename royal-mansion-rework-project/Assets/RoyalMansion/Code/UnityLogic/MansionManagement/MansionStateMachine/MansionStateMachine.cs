using RoyalMasion.Code.Infrastructure.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine.States;
using VContainer;
using RoyalMansion.Code.Extensions.Utils;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMansion.Code.UnityLogic.NPC;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine
{
    public class MansionStateMachine : IMansionStateMachine
    {
        public IExitableMansionState ActiveState { get; private set; }
        public GuestNPC NPC { get; set; }
        public Action<IExitableMansionState> StateChanged { get; set; }
        public UnitState LastTask { get; set; }

        private readonly Dictionary<Type, IExitableMansionState> _states;
        private readonly MansionStateMachineData _stateMachineData;


        public MansionStateMachine(MansionStateMachineData stateMachineData)
        {
            _states = new Dictionary<Type, IExitableMansionState>
            {
                [typeof(LockedState)] = new LockedState(),
                [typeof(EmptyState)] = new EmptyState(),
                [typeof(InUseState)] = new InUseState(),
                [typeof(CollectableState)] = new CollectableState(),
                [typeof(DirtyState)] = new DirtyState()
            };
            _stateMachineData = stateMachineData;
            foreach (var state in _states.Values)
            {
                state.SetupStateMachine(this);
            }
        }

        public void Enter<TState>() where TState : class, IMansionState
        {
            IMansionState state = ChangeState<TState>();
            StateChanged?.Invoke(_states[typeof(TState)]);
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IMansionPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            StateChanged?.Invoke(_states[typeof(TState)]);
            state.Enter(payload);
        }
        public void Stay()
        {
            ActiveState.Stay();
        }
        public MansionStateMachineData GetStateMachineData() => 
            _stateMachineData;
        public UnitState GetUnitStateEnum(Type type) => 
            MansionUtils.StateCorrelation[type];
        private TState ChangeState<TState>() where TState : class, IExitableMansionState
        {
            ActiveState?.Exit();

            var state = GetState<TState>();
            ActiveState = state;

            return state;
        }
          
        private TState GetState<TState>() where TState : class, IExitableMansionState
        {
            return _states[typeof(TState)] as TState;
        }
        public void CleanUp()
        {
            ActiveState = null;
            _states.Clear();
        }

    }
}
