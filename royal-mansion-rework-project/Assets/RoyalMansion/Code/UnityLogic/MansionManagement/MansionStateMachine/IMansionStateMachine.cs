using RoyalMansion.Code.UnityLogic.NPC;
using RoyalMasion.Code.Infrastructure.Data;
using RoyalMasion.Code.Infrastructure.StateMachine;
using System;

namespace RoyalMasion.Code.UnityLogic.MasionManagement.MansionStateMachine
{
    public interface IMansionStateMachine
    {
        IExitableMansionState ActiveState { get; }
        GuestNPC NPC {get; set;} 
        Action<IExitableMansionState> StateChanged { get; set; }
        void Enter<TMansionState>() where TMansionState : class, IMansionState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IMansionPayloadedState<TPayload>;
        void Stay();
        MansionStateMachineData GetStateMachineData();
        UnitState GetUnitStateEnum(Type type);
        UnitState LastTask { get; set; }
    }
    public interface IMansionState : IExitableMansionState
    {
        void Enter();
    }

    public interface IMansionPayloadedState<TPayload> : IExitableMansionState
    {
        void Enter(TPayload payload);
    }
    public interface IExitableMansionState
    {
        void Stay();
        void SetupStateMachine(IMansionStateMachine mansionStateMachine);
        void Exit();
    }
}
