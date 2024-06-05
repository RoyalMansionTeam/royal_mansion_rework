namespace RoyalMasion.Code.Infrastructure.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload levelID);
    }

    public interface IExitableState
    {
        void SetupStateMachine(IGameStateMachine gameStateMachine);
        void Exit();
    }
}